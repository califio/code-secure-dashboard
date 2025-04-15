using CodeSecure.Api.CI.Model;
using CodeSecure.Application;
using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Helpers;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Application.Module.Package;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Application.Module.Rule;
using CodeSecure.Application.Module.Scanner;
using CodeSecure.Application.Module.SourceControl;
using CodeSecure.Application.Services;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.CI.Service;

public class DefaultCiService(
    AppDbContext context,
    IPackageService packageService,
    IGlobalAlertManager globalAlertManager,
    IJiraWebHookService jiraWebHookService,
    ISmtpService smtpService,
    IRazorRender render,
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
        var sourceControl = (await context.CreateSourceControlsAsync(new SourceControls
        {
            Url = sourceUrl,
            Type = request.Source
        })).GetResult();
        // update project
        
        var project = await context.CreateProjectAsync(new Projects
        {
            Name = repoPath,
            RepoId = request.RepoId,
            RepoUrl = request.RepoUrl,
            SourceControlId = sourceControl.Id
        });
        // update scanner
        var scanner = (await context.CreateScannerAsync(new Scanners
        {
            Name = request.Scanner.Trim(),
            Type = request.Type,
        })).GetResult();
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
            ScanUrl = $"{Configuration.FrontendUrl}/#/project/{scan.ProjectId}/scan"
        };
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
        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstAsync(record => record.Id == scan.ProjectId);
        var projectSetting = context.GetProjectSettingsAsync(scan.ProjectId).Result.GetResult();
        foreach (var finding in request.Findings.Where(finding => finding.RuleId != null))
        {
            await context.CreateRuleAsync(new Rules
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
        var inputFindings = request.Findings
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
            .Select(newFinding => context.CreateFindingAsync(new Findings
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
            }).Result)
            .Where(x => x.IsSuccess)
            .Select(x => x.Value)
            .ToList();
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
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        #endregion
        #region Handle Fixed Finding

        var defaultBranches = projectSetting.GetDefaultBranches();
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
            
            if (scan.Commit.IsDefault || defaultBranches.Contains(scan.Commit.Branch))
            {
                fixedFinding.Status = FindingStatus.Fixed;
                fixedFinding.FixedAt = DateTime.UtcNow;
                context.Findings.Update(fixedFinding);
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
        var response = new CiUploadFindingResponse
        {
            NewFindings = newBranchFindings.Select(CiFinding.FromFinding),
            FixedFindings = fixedBranchFindings.Select(CiFinding.FromFinding),
            ConfirmedFindings = confirmedFindings.Select(CiFinding.FromFinding),
            NeedsTriageFindings = openFindings.Select(CiFinding.FromFinding),
            IsBlock = false,
            FindingUrl = $"{FrontendUrlHelper.ProjectFindingUrl(scan.ProjectId)}?commitId={scan.CommitId}&scanner={scan.Scanner?.Name}"
        };
        var isBlock = IsBlock(scan.ProjectId, response, scan.Scanner!.Type);
        response.IsBlock = isBlock;
        // send notification
        var projectAlertManager = new ProjectAlertManager(
            context:context, 
            projectId: project.Id, 
            smtpService: smtpService, 
            render:render);
        if (newBranchFindings.Count != 0)
        {
            var model = new AlertStatusFindingModel
            {
                SourceType = project.SourceControl!.Type,
                Project = project,
                Scanner = scan.Scanner,
                GitCommit = scan.Commit,
                Findings = newBranchFindings,
            };
            _ = globalAlertManager.AlertNewFinding(model);
            _ = projectAlertManager.AlertNewFinding(model);
        }

        if (fixedBranchFindings.Count != 0)
        {
            var model = new AlertStatusFindingModel
            {
                SourceType = project.SourceControl!.Type,
                Project = project,
                Scanner = scan.Scanner,
                GitCommit = scan.Commit,
                Findings = fixedBranchFindings,
            };
            _ = globalAlertManager.AlertFixedFinding(model);
            _ = projectAlertManager.AlertFixedFinding(model);
        }

        await jiraWebHookService.AlertScanCompleteAsync(new AlertScanCompleteModel
        {
            SourceType = project.SourceControl!.Type,
            Project = project,
            Scanner = scan.Scanner,
            GitCommit = scan.Commit,
            NewFindingCount = newBranchFindings.Count,
            ConfirmedFindingCount = confirmedFindings.Count,
            FixedFindingCount = fixedBranchFindings.Count,
            IsBlock = isBlock
        });
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
        var projectSetting = context.GetProjectSettingsAsync(scan.ProjectId).Result.GetResult();
        var scanUrl = $"{Configuration.FrontendUrl}/#/project/{scan.ProjectId}/scan/{scan.Id}";
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
            var result = await context.CreatePackageAsync(new Packages
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
            if (result.IsSuccess)
            {
                var package = result.Value;
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
        }
        // add or update package's dependencies
        if (request.PackageDependencies != null)
        {
            foreach (var record in request.PackageDependencies)
            {
                if (record.Dependencies != null)
                {
                    await packageService.AddDependenciesAsync(record.PkgId, record.Dependencies);
                }
            }
        }

        // add or update vulnerability
        if (request.Vulnerabilities != null)
        {
            foreach (var ciVulnerability in request.Vulnerabilities)
            {
                await packageService.AddVulnerabilityAsync(ciVulnerability.PkgId, new Vulnerabilities
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
            await packageService.UpdateRiskLevelAsync(package);
        }

        foreach (var package in packages)
        {
            await packageService.UpdateRiskImpactAsync(package);
            await packageService.UpdateRecommendationAsync(package);
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
                catch (Exception e)
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
            catch (Exception e)
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
        var defaultBranches = projectSetting.GetDefaultBranches();
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
                if (scan.Commit!.IsDefault || (defaultBranches.Contains(scan.Commit.Branch)))
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
    
    private bool IsBlock(Guid projectId, CiUploadFindingResponse response, ScannerType scannerType)
    {
        
        var setting = context.GetProjectSettingsAsync(projectId).Result.Value;
        var threshold = scannerType is ScannerType.Dependency or ScannerType.Container ? setting.GetScaSetting() : setting.GetSastSetting();
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