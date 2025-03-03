using CodeSecure.Api.CI.Model;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Metadata;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Extension;
using CodeSecure.Manager.EnvVariable.Model;
using CodeSecure.Manager.Finding;
using CodeSecure.Manager.Integration;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Integration.TicketTracker;
using CodeSecure.Manager.Package;
using CodeSecure.Manager.Project;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Rule;
using CodeSecure.Manager.Scanner;
using CodeSecure.Manager.SourceControl;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.CI.Service;

public class DefaultCiService(
    IHttpContextAccessor contextAccessor,
    AppDbContext context,
    IPackageManager packageManager,
    IScannerManager scannerManager,
    IFindingManager findingManager,
    IProjectManager projectManager,
    ISourceControlManager sourceControlManager,
    IAlertManager alertManager,
    IRuleManager ruleManager,
    ITicketTrackerManager ticketTrackerManager,
    ILogger<DefaultCiService> logger) : ICiService
{

    public async Task<CiScanInfo> InitScan(CiScanRequest request)
    {
        if ((request.Type is ScannerType.Sast or ScannerType.Dependency or ScannerType.Secret) == false)
        {
            throw new NotImplementedException();
        }
        // update source control
        var uri = new Uri(request.RepoUrl);
        var sourceUrl = uri.GetLeftPart(UriPartial.Authority);
        var repoPath = uri.AbsolutePath.TrimStart('/');
        var sourceControl = await sourceControlManager.CreateOrUpdateAsync(new SourceControls
        {
            Url = sourceUrl,
            Type = request.Source
        });
        // update project
        var project = await projectManager.CreateOrUpdateAsync(new Projects
        {
            Name = repoPath,
            RepoId = request.RepoId,
            RepoUrl = request.RepoUrl,
            SourceControlId = sourceControl.Id
        });
        // update scanner
        var scanner = await scannerManager.CreateOrUpdateAsync(new Scanners
        {
            Name = request.Scanner.Trim(),
            Type = request.Type,
        });
        // update scan
        var commitQuery = context.ProjectCommits.Where(record =>
            record.ProjectId == project.Id
            && record.Type == request.CommitType
            && record.Branch == request.CommitBranch);
        if (request.CommitType == CommitType.MergeRequest)
        {
            if (string.IsNullOrEmpty(request.TargetBranch)) throw new BadRequestException("Target branch invalid");

            commitQuery = commitQuery.Where(record => record.TargetBranch == request.TargetBranch);
        }

        var commit = await commitQuery.FirstOrDefaultAsync();
        if (commit == null)
        {
            commit = new GitCommits
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                IsDefault = request.IsDefault,
                Branch = request.CommitBranch,
                Type = request.CommitType,
                CommitHash = request.CommitHash,
                CommitTitle = request.ScanTitle,
                TargetBranch = request.TargetBranch,
                MergeRequestId = request.MergeRequestId
            };
            context.ProjectCommits.Add(commit);
            await context.SaveChangesAsync();
        }
        else
        {
            commit.CommitHash = request.CommitHash;
            commit.CommitTitle = request.ScanTitle;
            context.ProjectCommits.Update(commit);
            await context.SaveChangesAsync();
        }

        var scan = await context.Scans.FirstOrDefaultAsync(record =>
            record.ProjectId == project.Id &&
            record.ScannerId == scanner.Id &&
            record.CommitId == commit.Id);
        if (scan == null)
        {
            scan = new Scans
            {
                Id = Guid.NewGuid(),
                Status = ScanStatus.Running,
                JobUrl = request.JobUrl,
                StartedAt = DateTime.UtcNow,
                CompletedAt = null,
                Name = request.ScanTitle,
                Metadata = null,
                ProjectId = project.Id,
                ScannerId = scanner.Id,
                CommitId = commit.Id,
            };
            context.Scans.Add(scan);
        }
        else
        {
            scan.JobUrl = request.JobUrl;
            scan.StartedAt = DateTime.UtcNow;
            scan.CompletedAt = null;
            scan.Status = ScanStatus.Running;
            scan.Name = request.ScanTitle;
            context.Scans.Update(scan);
        }

        await context.SaveChangesAsync();

        return new CiScanInfo
        {
            ScanId = scan.Id,
            ScanUrl = $"{contextAccessor.FrontendUrl()}/#/project/{scan.ProjectId.ToString()}/scan"
        };
    }

    public async Task<List<EnvironmentVariable>> GetEnvironmentVariables(Guid scanId)
    {
        var scan = await context.Scans.FirstOrDefaultAsync(scan => scan.Id == scanId);
        if (scan == null)
        {
            return [];
        }

        return await context.ProjectEnvironmentVariables
            .Where(record => record.ProjectId == scan.ProjectId)
            .Select(record => new EnvironmentVariable
            {
                Name = record.Name,
                Value = record.Value
            }).ToListAsync();
    }

    public async Task UpdateScan(Guid scanId, UpdateCiScanRequest request)
    {
        var scan = await context.Scans.FirstOrDefaultAsync(scan => scan.Id == scanId);
        if (scan == null) throw new BadRequestException("Scan not found");
        
        if (request.Status != null && request.Status != scan.Status)
        {
            scan.Status = (ScanStatus)request.Status;
            if (request.Status == ScanStatus.Completed) scan.CompletedAt = DateTime.UtcNow;
        }
        
        if (!string.IsNullOrEmpty(request.Description))
        {
            if (request.Description.Length > 1024) request.Description = request.Description.Substring(0, 1024);
        
            scan.Description = request.Description;
        }
            
        context.Scans.Update(scan);
        await context.SaveChangesAsync();
    }

    public async Task<CiUploadFindingResponse> UploadFinding(CiUploadFindingRequest request)
    {
        var scan = await context.Scans
            .Include(scan => scan.Scanner)
            .Include(scan => scan.Commit)
            .FirstOrDefaultAsync(scan => scan.Id == request.ScanId);
        if (scan == null) throw new BadRequestException("Scan not found");

        if (scan.Status != ScanStatus.Running) throw new BadRequestException("Scan is not running");
        var response = await HandleCiFinding(scan, request.Findings);
        var isBlock = IsBlock(scan.ProjectId, response, scan.Scanner!.Type);
        response.IsBlock = isBlock;
        // send notification
        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstAsync(record => record.Id == scan.ProjectId);
        var frontendUrl = contextAccessor.FrontendUrl();
        var projectUrl = $"{frontendUrl}/#/project/{project.Id.ToString()}";
        var findingUrl =
            $"{projectUrl}/finding?commitId={scan.CommitId}&scanner={scan.Scanner!.Name}&type={scan.Scanner!.Type.ToString()}";
        var commitUrl = RepoHelpers.GetCommitUrl(project.SourceControl!.Type, project.RepoUrl, scan.Commit!.CommitHash ?? string.Empty);
        var mergeRequestUrl = GetMergeRequestUrl(project.SourceControl!.Type, project.RepoUrl, scan.Commit!.MergeRequestId);
        // send notification for security team
        if (response.NewFindings.Any())
        {
            await alertManager.AlertNewFinding(new NewFindingInfoModel
            {
                    ProjectName = project.Name,
                    ScannerName = scan.Scanner.Name,
                    ScannerType = scan.Scanner.Type.ToString(),
                    Findings = response.NewFindings.Select(item => new FindingModel
                        {
                            Name = item.Name,
                            Url = $"{Application.Config.FrontendUrl}/#/finding/{item.Id}",
                            Severity = item.Severity
                        })
                        .ToList(),
                    OpenFindingUrl = $"{findingUrl}&status=Open",
                    Action = scan.Commit!.Type,
                    CommitBranch = scan.Commit.Branch,
                    TargetBranch = scan.Commit.TargetBranch,
                    ScanName = scan.Scanner.Name,
                    CommitUrl = commitUrl,
                    MergeRequestUrl = mergeRequestUrl,
                    ProjectId = project.Id
            });
        }

        if (response.FixedFindings.Any())
        {
            await alertManager.AlertFixedFinding(new FixedFindingInfoModel
            {
                ProjectName = project.Name,
                Findings = response.FixedFindings.Select(item => new FindingModel
                    {
                        Name = item.Name,
                        Url = $"{Application.Config.FrontendUrl}/#/finding/{item.Id}",
                        Severity = item.Severity
                    })
                    .ToList(),
                FixedFindingUrl = $"{findingUrl}&status=Fixed",
                Action = scan.Commit!.Type,
                CommitBranch = scan.Commit.Branch,
                TargetBranch = scan.Commit.TargetBranch,
                ScanName = scan.Scanner.Name,
                CommitUrl = commitUrl,
                MergeRequestUrl = mergeRequestUrl,
                ProjectId = project.Id
            });
        }

        if (response.IsBlock)
        {
            await alertManager.AlertScanCompletedInfo(new ScanInfoModel
            {
                ProjectUrl = projectUrl,
                ProjectName = project.Name,
                ScanName = scan.Name,
                ScannerName = scan.Scanner!.Name,
                ScannerType = scan.Scanner!.Type.ToString()
                    .ToLower(),
                BlockStatus = response.IsBlock
                    ? "Block"
                    : "Pass",
                //Action = GetGitAction(scan.Commit!.Action),
                Action = scan.Commit!.Type,
                CommitBranch = scan.Commit.Branch,
                TargetBranch = scan.Commit.TargetBranch,
                MergeRequestUrl = mergeRequestUrl,
                FindingUrl = findingUrl,
                NewFinding = response.NewFindings.Count(),
                NeedsTriage = response.NeedsTriageFindings.Count(),
                Confirmed = response.ConfirmedFindings.Count(),
                CommitUrl = commitUrl,
                ProjectId = project.Id
            });
        }

        response.FindingUrl = findingUrl;
        return response;
    }

    public async Task<CiUploadDependencyResponse> UploadDependency(CiUploadDependencyRequest request)
    {
        var scan = await context.Scans
            .Include(scan => scan.Scanner)
            .Include(scan => scan.Commit)
            .FirstOrDefaultAsync(scan => scan.Id == request.ScanId);
        if (scan == null) throw new BadRequestException("scan not found");
        if (scan.Status != ScanStatus.Running) throw new BadRequestException("scan is not running");
        if (request.Packages == null)
        {
            return new CiUploadDependencyResponse
            {
                Packages = [],
                IsBlock = false
            };
        }

        // add or update packages
        List<Packages> packages = new();
        foreach (var item in request.Packages)
        {
            var package = await packageManager.CreateOrUpdateAsync(new Packages
            {
                PkgId = item.PkgId,
                Group = item.Group,
                Name = item.Name,
                Version = item.Version,
                Type = item.Type,
                RiskImpact = RiskImpact.None,
                RiskLevel = RiskLevel.None,
                FixedVersion = null,
                License = item.License
            });
            packages.Add(package);
        }

        // add or update package's dependencies
        if (request.PackageDependencies != null)
        {
            foreach (var record in request.PackageDependencies)
            {
                if (record.Dependencies != null)
                {
                    await packageManager.AddDependenciesAsync(record.PkgId, record.Dependencies);
                }
            }
        }

        // add or update vulnerability
        if (request.Vulnerabilities != null)
        {
            foreach (var ciVulnerability in request.Vulnerabilities)
            {
                await packageManager.AddVulnerabilityAsync(ciVulnerability.PkgId, new Vulnerabilities
                {
                    Metadata = JSONSerializer.Serialize(ciVulnerability.Metadata),
                    Name = ciVulnerability.Name,
                    Identity = ciVulnerability.Identity,
                    Description = ciVulnerability.Description,
                    Severity = ciVulnerability.Severity,
                    PkgName = ciVulnerability.PkgName,
                    PublishedAt = ciVulnerability.PublishedAt,
                    FixedVersion = ciVulnerability.FixedVersion
                });
            }
        }

        // update risk level of package
        foreach (var package in packages)
        {
            await packageManager.UpdateRiskLevelAsync(package);
        }

        foreach (var package in packages)
        {
            await packageManager.UpdateRiskImpactAsync(package);
            await packageManager.UpdateRecommendationAsync(package);
        }

        // add project packages.
        /*
         * we only update project packages if first scan or scan on master branch
         */
        // check scan this scanner on default branch
        var defaultCommitBranch = context.ProjectCommits.FirstOrDefault(record =>
            record.ProjectId == scan.ProjectId
            && record.IsDefault
            && record.Type == CommitType.Branch);
        var isScannedOnDefaultBranch = defaultCommitBranch != null &&
                                       context.Scans.Any(record =>
                                           record.ProjectId == scan.ProjectId &&
                                           record.CommitId == defaultCommitBranch.Id &&
                                           record.ScannerId == scan.ScannerId
                                       );

        if (isScannedOnDefaultBranch == false || scan.Commit!.IsDefault)
        {
            var ciProjectPackages =
                request.Packages.FindAll(package => string.IsNullOrEmpty(package.Location) == false);
            foreach (var pkg in ciProjectPackages)
            {
                await packageManager.AddToProjectAsync(scan.ProjectId, pkg.PkgId, pkg.Location!);
            }

            /*
             when upgrade package's version. we will remove the old package in database
             if the project packages in database not see in CI's project packages -> it's old package
            */
            // get all project packages
            var projectPackages = context.ProjectPackages
                .Where(record => record.ProjectId == scan.ProjectId)
                .ToList();
            foreach (var package in projectPackages)
            {
                // if DB 's project package not in CI's project packages
                if (!ciProjectPackages.Any(ciPackage =>
                        ciPackage.Location == package.Location &&
                        packageManager.FindByPkgIdAsync(ciPackage.PkgId).Result?.Id == package.PackageId))
                {
                    await packageManager.RemoveFromProjectAsync(scan.ProjectId, package.PackageId, package.Location);
                }
            }
        }

        var pPackages = await packageManager.FromProjectAsync(scan.ProjectId);
        var ciFindings = new List<CiFinding>();
        var ciPackageInfos = new List<CiPackageInfo>();
        foreach (var package in pPackages)
        {
            var vulnerabilities = await packageManager.GetVulnerabilitiesAsync(package.Package.Id);
            ciFindings.AddRange(vulnerabilities.Select(vulnerability => new CiFinding
            {
                RuleId = null,
                Identity = $"{vulnerability.Identity}_{package.Package.PkgId}_{package.Location}",
                Name = vulnerability.Name,
                Description = vulnerability.Description,
                Category = null,
                Recommendation = $"Upgrade {vulnerability.PkgName} to version {vulnerability.FixedVersion}",
                Severity = vulnerability.Severity,
                Location = new FindingLocation
                {
                    Path = package.Location
                },
                Metadata = JSONSerializer.Deserialize<FindingMetadata>(vulnerability.Metadata)
            }));
            ciPackageInfos.Add(new CiPackageInfo
            {
                Package = new CiPackage
                {
                    Id = package.Package.Id,
                    PkgId = package.Package.PkgId,
                    Group = package.Package.Group,
                    Name = package.Package.Name,
                    Version = package.Package.Version,
                    Type = package.Package.Type,
                    Location = package.Location
                },
                Vulnerabilities = vulnerabilities.Select(record => new CiVulnerability
                {
                    Identity = record.Identity,
                    Name = record.Name,
                    FixedVersion = record.FixedVersion,
                    Severity = record.Severity,
                    Description = string.Empty,
                    PkgId = string.Empty,
                    PkgName = string.Empty
                }).ToList()
            });
        }

        // var response = await HandleCiFinding(scan, ciFindings);
        // var isBlock = IsBlock(scan.ProjectId, response, scan.Scanner!.Type);
        if (isScannedOnDefaultBranch == false || scan.Commit!.IsDefault)
        {
            var report = (await projectManager.DependencyReportAsync(scan.ProjectId))!;
            if (report.Critical + report.High + report.Medium + report.Low > 0 && report.Packages.Any())
            {
                await alertManager.AlertVulnerableDependencies(new DependencyReportModel
                {
                    RepoUrl = report.RepoUrl,
                    RepoName = report.RepoName,
                    ProjectDependencyUrl = report.ProjectDependencyUrl,
                    Critical = report.Critical,
                    High = report.High,
                    Medium = report.Medium,
                    Low = report.Low,
                    Packages = report.Packages,
                    ProjectId = scan.ProjectId
                });
            }
            await CreateIssueTracker(scan.ProjectId);
        }

        return new CiUploadDependencyResponse
        {
            Packages = ciPackageInfos,
            IsBlock = false
        };
    }

    private async Task<CiUploadFindingResponse> HandleCiFinding(Scans scan, List<CiFinding> ciFindings)
    {
        foreach (var finding in ciFindings.Where(finding => finding.RuleId != null))
        {
            await ruleManager.CreateAsync(new Rules
            {
                Id = finding.RuleId!,
                ScannerId = scan.ScannerId,
                Status = RuleStatus.Enable,
                Confidence = RuleConfidence.Unknown,
            });
        }
        List<FindingStatus> activeStatus = [FindingStatus.Open, FindingStatus.Confirmed, FindingStatus.AcceptedRisk];
        // remove false positive finding & disable finding 
        var identityFalsePositiveFindings = context.Findings
            .Where(finding =>
                finding.ProjectId == scan.ProjectId &&
                finding.ScannerId == scan.ScannerId &&
                finding.Status == FindingStatus.Incorrect
            )
            .Select(finding => finding.Identity).ToHashSet();
        var disableRules = context.Rules
            .Where(rule => rule.ScannerId == scan.ScannerId && rule.Status == RuleStatus.Disable)
            .Select(rule => rule.Id).ToHashSet();
        var inputFindings = ciFindings
            .Where(finding => 
                identityFalsePositiveFindings.Contains(finding.Identity) == false &&
                disableRules.Contains(finding.RuleId) == false
            ).ToHashSet();
        // load active scan findings
        var currentBranchFindings = context.ScanFindings
            .Include(record => record.Finding)
            .Where(record => record.ScanId == scan.Id && activeStatus.Contains(record.Status))
            .Select(record => record.Finding!).ToHashSet();
        if (scan.Commit!.Type == CommitType.MergeRequest)
        {
            var targetScanId = context.Scans
                .Where(record =>
                    record.ProjectId == scan.ProjectId
                    && record.ScannerId == scan.ScannerId
                    && record.Commit!.Type == CommitType.Branch
                    && record.Commit.Branch == scan.Commit.TargetBranch)
                .Select(record => record.Id).FirstOrDefault();
            context.ScanFindings
                .Include(record => record.Finding)
                .Where(record => record.ScanId == targetScanId && activeStatus.Contains(record.Status))
                .Select(record => record.Finding!)
                .ToList().ForEach(finding =>
                {
                    currentBranchFindings.Add(finding);
                });
        }
        
        // new branch findings 
        var newBranchFindings = inputFindings
            .Where(inputFinding => currentBranchFindings.Any(branchFinding => branchFinding.Identity == inputFinding.Identity) == false)
            .Select(newFinding => findingManager.CreateAsync(new Findings
            {
                Identity = newFinding.Identity,
                RuleId = newFinding.RuleId,
                Name = newFinding.Name,
                Description = newFinding.Description,
                Recommendation = newFinding.Recommendation,
                Status = FindingStatus.Open,
                Severity = newFinding.Severity,
                Metadata = JSONSerializer.Serialize(newFinding.Metadata),
                ProjectId = scan.ProjectId,
                ScannerId = scan.ScannerId,
                Location = newFinding.Location?.Path,
                Snippet = newFinding.Location?.Snippet,
                StartLine = newFinding.Location?.StartLine,
                EndLine = newFinding.Location?.EndLine,
                StartColumn = newFinding.Location?.StartColumn,
                EndColumn = newFinding.Location?.EndColumn,
                Category = newFinding.Category
            }).Result).ToList();
        // fixed branch findings
        var fixedBranchFindings = currentBranchFindings
            .Where(finding => 
                inputFindings.Any(inputFinding => inputFinding.Identity == finding.Identity) == false &&
                disableRules.Contains(finding.RuleId) == false
            )
            .ToList();

        #region Handle Finding
        #region Handle New Finding
        foreach (var finding in newBranchFindings)
        {
            try
            {
                context.ScanFindings.Add(new ScanFindings
                {
                    ScanId = scan.Id,
                    FindingId = finding.Id,
                    Status = finding.Status,
                    CommitHash = scan.Commit!.CommitHash ?? string.Empty
                });
                await context.SaveChangesAsync();
                context.FindingActivities.Add(FindingActivities.OpenFinding(finding.Id, scan.CommitId));
                await context.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        #endregion
        #region Handle Fixed Finding
        foreach (var fixedFinding in fixedBranchFindings)
        {
            // update status (fixed) of finding on this scan
            await context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == fixedFinding.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.Status, FindingStatus.Fixed));
            // add change status activity of finding on scan
            context.FindingActivities.Add(FindingActivities.FixedFinding(fixedFinding.Id, scan.CommitId));
            // fixed on default branch or the finding only effected on one branch -> fixed finding
            if (scan.Commit.IsDefault || context.ScanFindings.Count(record => record.FindingId == fixedFinding.Id) == 1)
            {
                fixedFinding.Status = FindingStatus.Fixed;
                context.Findings.Update(fixedFinding);
                // todo: notify for security team to recheck 
            }
            await context.SaveChangesAsync();
        }
        #endregion
        #region Handle Reopen Finding
        // reopen branch finding: finding fixed in the past but found again in this scan 
        var reopenFindings = context.ScanFindings
            .Include(record => record.Finding)
            .Where(record => record.ScanId == scan.Id && record.Status == FindingStatus.Fixed)
            .Select(record => record.Finding!)
            .ToList()
            .Where(fixedFinding => inputFindings.Any(inputFinding => inputFinding.Identity == fixedFinding.Identity))
            .ToList();
        foreach (var finding in reopenFindings)
        {
            await context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == finding.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.Status, FindingStatus.Confirmed));
            // add reopen activity of finding on scan
            context.FindingActivities.Add(FindingActivities.ReopenFinding(finding.Id, scan.CommitId));
            if (scan.Commit.IsDefault || context.ScanFindings.Count(record => record.FindingId == finding.Id) == 1)
            {
                finding.Status = FindingStatus.Confirmed;
                context.Findings.Update(finding);
            }
            await context.SaveChangesAsync();
        }
        #endregion
        #endregion
        // the scan findings that found before
        var knownFindings = currentBranchFindings
            .Where(finding => inputFindings.Any(inputFinding => inputFinding.Identity == finding.Identity)).ToList();
        // update latest commit sha to known finding
        foreach (var finding in knownFindings)
        {
            await context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == finding.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.CommitHash, scan.Commit.CommitHash));
        }
        // open findings but not verified -> need to verify
        var openFindings = knownFindings.Where(finding => finding.Status == FindingStatus.Open).ToList();
        // confirmed findings but is not fixed -> need to fix
        var confirmedFindings = knownFindings.Where(finding => finding.Status == FindingStatus.Confirmed).ToList();
        return new CiUploadFindingResponse
        {
            NewFindings = newBranchFindings.Select(CiFinding.FromFinding),
            FixedFindings = fixedBranchFindings.Select(CiFinding.FromFinding),
            ConfirmedFindings = confirmedFindings.Select(CiFinding.FromFinding),
            NeedsTriageFindings = openFindings.Select(CiFinding.FromFinding),
            IsBlock = false,
            FindingUrl = ""
        };
    }

    private string? GetMergeRequestUrl(SourceType sourceType, string repoUrl, string? mergeRequestId)
    {
        if (mergeRequestId == null) return null;
        if (sourceType == SourceType.GitLab) return $"{repoUrl}/-/merge_requests/{mergeRequestId}";
        // todo: add other source
        return null;
    }
    
    private bool IsBlock(Guid projectId, CiUploadFindingResponse response, ScannerType scannerType)
    {
        var setting = projectManager.GetProjectSettingAsync(projectId).Result;
        ThresholdSetting threshold;
        if (scannerManager.IsScaScanner(scannerType))
        {
            threshold = setting.ScaSetting;
        }
        else
        {
            threshold = setting.SastSetting;
        }

        if (threshold.Mode == ThresholdMode.MonitorOnly)
        {
            return false;
        }

        List<CiFinding> findings = new();
        if (threshold.Mode == ThresholdMode.BlockOnConfirmation)
        {
            findings.AddRange(response.ConfirmedFindings);
        }
        else
        {
            findings.AddRange(response.NewFindings);
            findings.AddRange(response.NeedsTriageFindings);
            findings.AddRange(response.ConfirmedFindings);
        }

        var mSeverities = new Dictionary<FindingSeverity, int>()
        {
            { FindingSeverity.Critical, 0 },
            { FindingSeverity.High, 0 },
            { FindingSeverity.Medium, 0 },
            { FindingSeverity.Low, 0 },
            { FindingSeverity.Info, 0 },
        };
        var now = DateTime.UtcNow;
        foreach (var finding in findings)
        {
            if (finding.FixDeadline == null || finding.FixDeadline < now)
            {
                mSeverities[finding.Severity] += 1;
            }
        }

        if (threshold.Critical > 0 && mSeverities[FindingSeverity.Critical] >= threshold.Critical)
        {
            return true;
        }

        if (threshold.High > 0 && mSeverities[FindingSeverity.High] >= threshold.High)
        {
            return true;
        }

        if (threshold.Medium > 0 && mSeverities[FindingSeverity.Medium] >= threshold.Medium)
        {
            return true;
        }

        if (threshold.Low > 0 && mSeverities[FindingSeverity.Low] >= threshold.Low)
        {
            return true;
        }

        return false;
    }
    
    private async Task CreateIssueTracker(Guid projectId)
    {
        var projectPackages = await context.ProjectPackages
            .Include(record => record.Package)
            .Include(record => record.Project)
            .Where(record => record.ProjectId == projectId)
            .ToListAsync();
        foreach (var package in projectPackages.Where(item => item.TicketId == null && !string.IsNullOrEmpty(item.Package!.FixedVersion)))
        {
            var vulnerabilities = await context.PackageVulnerabilities
                .Include(record => record.Vulnerability)
                .Where(record => record.PackageId == package.PackageId)
                .Select(record => record.Vulnerability!)
                .ToListAsync();
            await ticketTrackerManager.CreateTicketAsync(new ScaTicket
            {
                Location = package.Location,
                Project = package.Project!,
                Package = package.Package!,
                Vulnerabilities = vulnerabilities
            });
        }
    }
}