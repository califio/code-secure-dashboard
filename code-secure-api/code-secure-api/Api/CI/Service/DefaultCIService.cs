using CodeSecure.Api.CI.Model;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
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
            && record.Type == request.GitAction
            && record.Branch == request.CommitBranch);
        if (request.GitAction == CommitType.MergeRequest)
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
                Type = request.GitAction,
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

    public async Task<ScanDependencyResult> UploadDependency(CiUploadDependencyRequest request)
    {
        var scan = await context.Scans
            .Include(scan => scan.Scanner)
            .Include(scan => scan.Commit)
            .FirstOrDefaultAsync(scan => scan.Id == request.ScanId);
        if (scan == null) throw new BadRequestException("scan not found");
        if (scan.Status != ScanStatus.Running) throw new BadRequestException("scan is not running");
        var scanUrl = $"{Application.Config.FrontendUrl}/#/project/{scan.ProjectId}/scan/{scan.Id}";
        if (request.Packages == null)
        {
            return new ScanDependencyResult
            {
                Packages = [],
                IsBlock = false,
                Scanner = scan.Scanner!.Name,
                ScanUrl = scanUrl
            };
        }
        // add or update packages
        List<Packages> packages = [];
        List<ProjectPackages> requestProjectPackages = [];
        foreach (var item in request.Packages)
        {
            var package = await packageManager.CreateAsync(new Packages
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
            if (string.IsNullOrEmpty(item.Location) == false)
            {
                requestProjectPackages.Add(new ProjectPackages
                {
                    Id = Guid.NewGuid(),
                    ProjectId = scan.ProjectId,
                    PackageId = package.Id,
                    Location = item.Location,
                    Status = PackageStatus.Open,
                });
            }
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
        var projectPackages = context.ProjectPackages
            .Where(record => record.ProjectId == scan.ProjectId)
            .ToList();
        List<ProjectPackages> projectPackagesOfCurrentScan = [];
        foreach (var requestProjectPackage in requestProjectPackages)
        {
            var projectPackage = projectPackages.FirstOrDefault(item => 
                    item.PackageId == requestProjectPackage.PackageId && 
                    item.Location == requestProjectPackage.Location);
            if (projectPackage == null)
            {
                try
                {
                    context.ProjectPackages.Add(requestProjectPackage);
                    await context.SaveChangesAsync();
                    projectPackagesOfCurrentScan.Add(requestProjectPackage);
                }
                catch (System.Exception e)
                {
                    logger.LogError(e.Message);
                }
            }
            else
            {
                projectPackagesOfCurrentScan.Add(projectPackage);
            }
        }
        // scan project packages
        var projectPackagesOfLastScan = context.ProjectPackages
            .Include(record => record.Package)
            .Where(record => context.ScanProjectPackages.Any(scanProject =>
                record.Id == scanProject.ProjectPackageId && scanProject.ScanId == scan.Id))
            .ToList();
        var newProjectPackagesOfScan = projectPackagesOfCurrentScan.Where(package => projectPackagesOfLastScan.Any(record => 
                record.PackageId == package.PackageId && record.Location == package.Location) == false)
            .ToList();
        // add new package of scan
        foreach (var projectPackage in newProjectPackagesOfScan)
        {
            try
            {
                context.ScanProjectPackages.Add(new ScanProjectPackages
                {
                    Id = Guid.NewGuid(),
                    ScanId = scan.Id,
                    ProjectPackageId = projectPackage.Id,
                    Status = PackageStatus.Open,
                });
                await context.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        var fixedPackageOfScan = projectPackagesOfLastScan.Where(package => 
                projectPackagesOfCurrentScan.Any(record => 
                    record.PackageId == package.PackageId && 
                    record.Location == package.Location) == false)
            .ToList();
        // update status of scan
        foreach (var fixedProjectPackage in fixedPackageOfScan)
        {
            if (fixedProjectPackage.Package!.RiskLevel == RiskLevel.None)
            {
                await context.ScanProjectPackages
                    .Where(record => record.ProjectPackageId == fixedProjectPackage.Id && record.ScanId == scan.Id)
                    .ExecuteDeleteAsync();
            }
            else
            {
                await context.ScanProjectPackages
                    .Where(record => record.ProjectPackageId == fixedProjectPackage.Id && record.ScanId == scan.Id)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(column => column.Status, PackageStatus.Fixed)
                        .SetProperty(column => column.FixedAt, DateTime.UtcNow)
                    );
                if (scan.Commit!.IsDefault || scan.Commit!.Branch == "main" || scan.Commit.Branch == "master" ||
                    context.ScanProjectPackages.Count(record => record.ProjectPackageId == fixedProjectPackage.Id) == 1)
                {
                    await context.ProjectPackages.Where(record => record.Id == fixedProjectPackage.Id)
                        .ExecuteUpdateAsync(setter => setter
                            .SetProperty(column => column.Status, PackageStatus.Fixed)
                            .SetProperty(column => column.FixedAt, DateTime.UtcNow)
                        );
                }
            }
        }
        
        var projectPackagesOfScan = context.ProjectPackages
            .Include(record => record.Package)
            .Where(record => context.ScanProjectPackages.Any(scanProject =>
                record.Id == scanProject.ProjectPackageId && scanProject.ScanId == scan.Id))
            .Distinct()
            .ToList();
        List<CiPackageInfo> packagesInfo = [];
        foreach (var package in projectPackagesOfScan)
        {
            var vulnerabilities = context.PackageVulnerabilities
                .Include(record => record.Vulnerability)
                .Where(record => record.PackageId == package.PackageId)
                .Select(record => new VulnerabilityInfo
                {
                    Id = record.Vulnerability!.Identity,
                    Name = record.Vulnerability.Name,
                    Description = record.Vulnerability.Description,
                    Severity = record.Vulnerability.Severity,
                    FixedVersion = record.Vulnerability.FixedVersion,
                    PublishedAt = record.Vulnerability.PublishedAt
                })
                .OrderByDescending(record => record.Severity)
                .ToList();
            packagesInfo.Add(new CiPackageInfo
            {
                PkgId = package.Package!.PkgId,
                Group = package.Package.Group,
                Name = package.Package.Name,
                Version = package.Package.Version,
                Type = package.Package.Type,
                Location = package.Location,
                License = package.Package.License,
                Vulnerabilities = vulnerabilities
            });
        }
        return new ScanDependencyResult
        {
            Packages = packagesInfo,
            IsBlock = false,
            Scanner = scan.Scanner!.Name,
            ScanUrl = scanUrl
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
                finding.RuleId != null && disableRules.Contains(finding.RuleId) == false
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
                    && record.Commit!.Type == CommitType.CommitBranch
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
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(column => column.Status, FindingStatus.Fixed)
                    .SetProperty(column => column.FixedAt, DateTime.UtcNow)
                );
            // add change status activity of finding on scan
            context.FindingActivities.Add(FindingActivities.FixedFinding(fixedFinding.Id, scan.CommitId));
            // fixed on default branch or the finding only effected on one branch -> fixed finding
            if (scan.Commit.IsDefault || scan.Commit!.Branch == "main" || scan.Commit.Branch == "master" ||
                context.ScanFindings.Count(record => record.FindingId == fixedFinding.Id) == 1)
            {
                fixedFinding.Status = FindingStatus.Fixed;
                fixedFinding.FixedAt = DateTime.UtcNow;
                context.Findings.Update(fixedFinding);
                // todo: notify 
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
                finding.FixedAt = null;
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
}