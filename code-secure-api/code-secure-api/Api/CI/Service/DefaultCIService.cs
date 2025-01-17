using CodeSecure.Api.CI.Model;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Metadata;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Extension;
using CodeSecure.Manager.Container;
using CodeSecure.Manager.EnvVariable.Model;
using CodeSecure.Manager.Finding;
using CodeSecure.Manager.Notification;
using CodeSecure.Manager.Notification.Model;
using CodeSecure.Manager.Package;
using CodeSecure.Manager.Project;
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
    IContainerManager containerManager,
    IProjectManager projectManager,
    ISourceControlManager sourceControlManager,
    INotification notification,
    ILogger<DefaultCiService> logger) : ICiService
{
    public async Task<CiScanInfo> InitScan(CiScanRequest request)
    {
        if (request.Type == ScannerType.Container && string.IsNullOrEmpty(request.ContainerImage))
        {
            throw new BadRequestException("scan container require container image");
        }

        // update source control
        var uri = new Uri(request.RepoUrl);
        var sourceUrl = uri.GetLeftPart(UriPartial.Authority);
        var sourceControl = await sourceControlManager.CreateOrUpdateAsync(new SourceControls
        {
            Url = sourceUrl,
            Type = request.Source
        });
        // update project
        var project = await projectManager.CreateOrUpdateAsync(new Projects
        {
            Name = request.RepoName,
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
            && record.Action == request.GitAction
            && record.Branch == request.CommitBranch);
        if (request.GitAction == GitAction.MergeRequest)
        {
            if (string.IsNullOrEmpty(request.TargetBranch)) throw new BadRequestException("target branch invalid");

            commitQuery = commitQuery.Where(record => record.TargetBranch == request.TargetBranch);
        }

        var commit = await commitQuery.FirstOrDefaultAsync();
        if (commit == null)
        {
            commit = new ProjectCommits
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                IsDefault = request.IsDefault,
                Branch = request.CommitBranch,
                Action = request.GitAction,
                TargetBranch = request.TargetBranch
            };
            context.ProjectCommits.Add(commit);
            await context.SaveChangesAsync();
        }

        Containers? container = null;
        if (request.Type == ScannerType.Container)
        {
            container = await containerManager.CreateAsync(new Containers
            {
                Name = request.ContainerImage!
            });
            request.ScanTitle = $"Scan container image {container.Name}";
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
                CommitHash = request.CommitHash,
                MergeRequestId = request.MergeRequestId,
                Metadata = null,
                ProjectId = project.Id,
                ScannerId = scanner.Id,
                CommitId = commit.Id,
                ContainerId = container?.Id
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
            scan.CommitHash = request.CommitHash;
            scan.MergeRequestId = request.MergeRequestId;
            scan.ContainerId = container?.Id;
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
        if (scan == null) throw new BadRequestException("scan not found");

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
        if (scan == null) throw new BadRequestException("scan not found");

        if (scan.Status != ScanStatus.Running) throw new BadRequestException("scan is not running");
        var response = await HandleCiFinding(scan, request.Findings);
        var isBlock = IsBlock(scan.ProjectId, response, scan.Scanner!.Type);
        response.IsBlock = isBlock;
        // send notification
        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstAsync(record => record.Id == scan.ProjectId);
        var members = await projectManager.GetMembersAsync(scan.ProjectId);
        var frontendUrl = contextAccessor.FrontendUrl();
        var projectUrl = $"{frontendUrl}/#/project/{project.Id.ToString()}";
        var findingUrl = $"{projectUrl}/finding?commitId={scan.CommitId}&scanner={scan.Scanner!.Name}&type={scan.Scanner!.Type.ToString()}";
        var commitUrl = GetCommitUrl(project.SourceControl!.Type, project.RepoUrl, scan.CommitHash);
        var mergeRequestUrl = GetMergeRequestUrl(project.SourceControl!.Type, project.RepoUrl, scan.MergeRequestId);
        // send notification for security team
        if (response.NewFindings.Any())
        {
            notification.PushNewFindingInfo(
                members.FindAll(item => item.Role != ProjectRole.Developer)
                    .Select(item => item.Email).ToList(),
                new NewFindingInfoModel
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
                    Action = scan.Commit!.Action,
                    CommitBranch = scan.Commit.Branch,
                    TargetBranch = scan.Commit.TargetBranch,
                    ScanName = scan.Scanner.Name,
                    CommitUrl = commitUrl,
                    MergeRequestUrl = mergeRequestUrl
                });
        }

        if (response.FixedFindings.Any())
        {
            notification.PushFixedFindingInfo(
                members.FindAll(item => item.Role != ProjectRole.Developer)
                    .Select(item => item.Email).ToList(),
                new FixedFindingInfoModel
                {
                    ProjectName = project.Name,
                    Findings = response.NewFindings.Select(item => new FindingModel
                        {
                            Name = item.Name,
                            Url = $"{Application.Config.FrontendUrl}/#/finding/{item.Id}",
                            Severity = item.Severity
                        })
                        .ToList(),
                    FixedFindingUrl = $"{findingUrl}&status=Fixed",
                    Action = scan.Commit!.Action,
                    CommitBranch = scan.Commit.Branch,
                    TargetBranch = scan.Commit.TargetBranch,
                    ScanName = scan.Scanner.Name,
                    CommitUrl = commitUrl,
                    MergeRequestUrl = mergeRequestUrl
                });
        }

        if (response.IsBlock)
        {
            notification.PushScanInfo(members.Select(item => item.Email), new ScanInfoModel
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
                Action = scan.Commit!.Action,
                CommitBranch = scan.Commit.Branch,
                TargetBranch = scan.Commit.TargetBranch,
                MergeRequestUrl = mergeRequestUrl,
                FindingUrl = findingUrl,
                NewFinding = response.NewFindings.Count(),
                NeedsTriage = response.NeedsTriageFindings.Count(),
                Confirmed = response.ConfirmedFindings.Count(),
                CommitUrl = commitUrl
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
            && record.Action == GitAction.CommitBranch);
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

        var response = await HandleCiFinding(scan, ciFindings);
        var isBlock = IsBlock(scan.ProjectId, response, scan.Scanner!.Type);
        if (isScannedOnDefaultBranch == false || scan.Commit!.IsDefault)
        {
            var report = (await projectManager.DependencyReportAsync(scan.ProjectId))!;
            if (report.Critical + report.High + report.Medium + report.Low > 0 && report.Packages.Any())
            {
                var members = await projectManager.GetMembersAsync(scan.ProjectId);
                await notification.PushDependencyReport(
                    members.Select(item => item.Email),
                    new DependencyReportModel
                    {
                        RepoUrl = report.RepoUrl,
                        RepoName = report.RepoName,
                        ProjectDependencyUrl = report.ProjectDependencyUrl,
                        Critical = report.Critical,
                        High = report.High,
                        Medium = report.Medium,
                        Low = report.Low,
                        Packages = report.Packages
                    }, null);
            }
        }

        return new CiUploadDependencyResponse
        {
            Packages = ciPackageInfos,
            IsBlock = isBlock
        };
    }

    private async Task<CiUploadFindingResponse> HandleCiFinding(Scans scan, List<CiFinding> ciFindings)
    {
        // project findings
        var projectFindings = await context.Findings.Where(finding =>
            finding.ProjectId == scan.ProjectId &&
            finding.ScannerId == scan.ScannerId).ToListAsync();
        var mProjectFindings = new Dictionary<string, Findings>();
        foreach (var finding in projectFindings)
        {
            mProjectFindings[finding.Identity] = finding;
        }
        // branch findings
        List<Findings> branchFindings;
        Guid? scanId = null;
        if (scan.Commit!.Action == GitAction.MergeRequest)
        {
            // get scan id of target scan
            var scanTargetBranch = await context.Scans
                .Include(record => record.Commit)
                .FirstOrDefaultAsync(record =>
                    record.ProjectId == scan.ProjectId
                    && record.ScannerId == scan.ScannerId
                    && record.Commit!.Action == GitAction.CommitBranch
                    && record.Commit.Branch == scan.Commit.TargetBranch);
            if (scanTargetBranch != null) scanId = scanTargetBranch.Id;
        }
        else
        {
            scanId = scan.Id;
        }

        if (scanId != null)
            branchFindings = await context.ScanFindings
                .Include(record => record.Finding)
                .Where(record => record.ScanId == scanId)
                .Select(record => record.Finding!).ToListAsync();
        else
            branchFindings = [];

        var mBranchFindings = branchFindings.Select(item =>
            new KeyValuePair<string, Findings>(item.Identity, item)).ToDictionary();
        // new branch findings 
        var newBranchFindings = new List<Findings>();
        foreach (var newBranchFinding in ciFindings.Where(finding => !mBranchFindings.ContainsKey(finding.Identity)))
        {
            mProjectFindings.TryGetValue(newBranchFinding.Identity, out var projectFinding);
            if (projectFinding != null)
            {
                // ignore false positive (incorrect), accepted risk findings.
                if (projectFinding.Status is (FindingStatus.Incorrect or FindingStatus.AcceptedRisk)) continue;
                if (projectFinding.Status == FindingStatus.Fixed) projectFinding.Status = FindingStatus.Confirmed;

                newBranchFindings.Add(projectFinding);
            }
            else
            {
                var newFinding = await findingManager.CreateAsync(new Findings
                {
                    Identity = newBranchFinding.Identity,
                    RuleId = newBranchFinding.RuleId,
                    Name = newBranchFinding.Name,
                    Description = newBranchFinding.Description,
                    Recommendation = newBranchFinding.Recommendation,
                    Status = FindingStatus.Open,
                    Severity = newBranchFinding.Severity,
                    Metadata = JSONSerializer.Serialize(newBranchFinding.Metadata),
                    ProjectId = scan.ProjectId,
                    ScannerId = scan.ScannerId,
                    Location = newBranchFinding.Location?.Path,
                    Snippet = newBranchFinding.Location?.Snippet,
                    StartLine = newBranchFinding.Location?.StartLine,
                    EndLine = newBranchFinding.Location?.EndLine,
                    StartColumn = newBranchFinding.Location?.StartColumn,
                    EndColumn = newBranchFinding.Location?.EndColumn,
                    Category = newBranchFinding.Category
                });
                newBranchFindings.Add(newFinding);
            }
        }

        await context.SaveChangesAsync();
        if (newBranchFindings.Count > 0)
        {
            // add new scan findings
            var newScanFindings = newBranchFindings.Select(finding => new ScanFindings
            {
                ScanId = scan.Id,
                FindingId = finding.Id,
                Status = finding.Status,
                CommitHash = scan.CommitHash
            }).ToList();
            foreach (var finding in newBranchFindings)
            {
                var entry = context.ChangeTracker.Entries<ScanFindings>().FirstOrDefault(entity =>
                    entity.Entity.ScanId == scanId && entity.Entity.FindingId == finding.Id);
                if (entry == null || entry.State != EntityState.Added)
                {
                    context.ScanFindings.Add(new ScanFindings
                    {
                        ScanId = scan.Id,
                        FindingId = finding.Id,
                        Status = finding.Status,
                        CommitHash = scan.CommitHash
                    });
                }
            }
            context.ScanFindings.AddRange(newScanFindings);
            await context.SaveChangesAsync();
            // add activity of finding
            newScanFindings.ForEach(scanFinding =>
            {
                context.FindingActivities.Add(new FindingActivities
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Comment = null,
                    Type = FindingActivityType.Open,
                    Metadata = JSONSerializer.Serialize(new FindingActivityMetadata
                    {
                        ScanInfo = new FindingScanActivity
                        {
                            Branch = scan.Commit.Branch,
                            Action = scan.Commit.Action,
                            TargetBranch = scan.Commit.TargetBranch,
                            IsDefault = scan.Commit.IsDefault
                        }
                    }),
                    FindingId = scanFinding.FindingId
                });
            });
            await context.SaveChangesAsync();
        }

        // fixed findings
        var mCiFindings = ciFindings.Select(finding =>
                new KeyValuePair<string, CiFinding>(finding.Identity, finding))
            .ToDictionary();
        var fixedBranchFindings = branchFindings.FindAll(finding =>
            finding.Status != FindingStatus.Fixed && !mCiFindings.ContainsKey(finding.Identity)).ToList();
        if (fixedBranchFindings.Count > 0)
            fixedBranchFindings.ForEach(fixedFinding =>
            {
                // update status (fixed) of finding on this scan
                context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == fixedFinding.Id)
                    .ExecuteUpdate(setter => setter.SetProperty(record => record.Status, FindingStatus.Fixed));
                // add change status activity of finding on scan
                context.FindingActivities.Add(new FindingActivities
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Comment = null,
                    Type = FindingActivityType.Fixed,
                    Metadata = JSONSerializer.Serialize(new FindingActivityMetadata
                    {
                        ScanInfo = new FindingScanActivity
                        {
                            Branch = scan.Commit.Branch,
                            Action = scan.Commit.Action,
                            TargetBranch = scan.Commit.TargetBranch,
                            IsDefault = scan.Commit.IsDefault
                        }
                    }),
                    FindingId = fixedFinding.Id
                });
                // fixed on default branch or the finding only effected on one branch -> fixed finding
                if (scan.Commit.IsDefault ||
                    context.ScanFindings.Count(record => record.FindingId == fixedFinding.Id) == 1)
                {
                    fixedFinding.Status = FindingStatus.Fixed;
                    context.Findings.Update(fixedFinding);
                    // todo: notify for security team to recheck 
                }

                context.SaveChanges();
            });

        // the new findings that found before
        var oldFindings = ciFindings.Where(finding => mBranchFindings.ContainsKey(finding.Identity))
            .Select(finding => mBranchFindings[finding.Identity]).ToList();

        // open findings but not verified -> need to verify
        var openFindings = oldFindings.Where(finding => finding.Status == FindingStatus.Open).ToList();

        // confirmed findings but is not fixed -> need to fix
        var confirmedFindings = oldFindings.Where(finding => finding.Status == FindingStatus.Confirmed).ToList();
        oldFindings.ForEach(finding =>
        {
            // ignore on merge request
            if (scan.Commit.Action != GitAction.MergeRequest)
                // update last commit. ignore false positive (incorrect), accepted risk (ignore) findings 
                if (finding.Status is FindingStatus.Open or FindingStatus.Confirmed or FindingStatus.Fixed)
                {
                    var scanFinding = context.ScanFindings.FirstOrDefault(record =>
                        record.ScanId == scan.Id && record.FindingId == finding.Id);
                    if (scanFinding == null)
                    {
                        scanFinding = new ScanFindings
                        {
                            ScanId = scan.Id,
                            FindingId = finding.Id,
                            Status = finding.Status,
                            CommitHash = scan.CommitHash
                        };
                        if (finding.Status == FindingStatus.Fixed) scanFinding.Status = FindingStatus.Confirmed;

                        context.ScanFindings.Add(scanFinding);
                    }
                    else
                    {
                        scanFinding.CommitHash = scan.CommitHash;
                        if (finding.Status == FindingStatus.Fixed)
                            //reopen finding
                            scanFinding.Status = FindingStatus.Confirmed;

                        context.ScanFindings.Update(scanFinding);
                    }

                    context.SaveChanges();
                }

            // finding fixed in the past but found again in this scan -> reopen finding
            if (finding.Status == FindingStatus.Fixed)
            {
                if (scan.Commit.Action == GitAction.MergeRequest)
                {
                    newBranchFindings.Add(finding);
                    context.ScanFindings.Add(new ScanFindings
                    {
                        ScanId = scan.Id,
                        FindingId = finding.Id,
                        Status = FindingStatus.Confirmed,
                        CommitHash = scan.CommitHash
                    });
                }
                else
                {
                    openFindings.Add(finding);
                    // add reopen activity of finding on scan
                    context.FindingActivities.Add(new FindingActivities
                    {
                        Id = Guid.NewGuid(),
                        UserId = null,
                        Comment = "Reopen finding",
                        Type = FindingActivityType.Reopen,
                        Metadata = JSONSerializer.Serialize(new FindingActivityMetadata
                        {
                            ScanInfo = new FindingScanActivity
                            {
                                Branch = scan.Commit.Branch,
                                Action = scan.Commit.Action,
                                TargetBranch = scan.Commit.TargetBranch,
                                IsDefault = scan.Commit.IsDefault
                            }
                        }),
                        FindingId = finding.Id
                    });
                    if (scan.Commit.IsDefault ||
                        context.ScanFindings.Count(record => record.FindingId == finding.Id) == 1)
                    {
                        finding.Status = FindingStatus.Confirmed;
                        context.Findings.Update(finding);
                    }
                }

                context.SaveChanges();
            }
        });
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

    private string GetGitAction(GitAction action)
    {
        if (action == GitAction.CommitTag) return "commit_tag";

        if (action == GitAction.MergeRequest) return "merge_request";

        return "commit_branch";
    }

    private string? GetMergeRequestUrl(SourceType sourceType, string repoUrl, string? mergeRequestId)
    {
        if (mergeRequestId == null) return null;
        if (sourceType == SourceType.GitLab) return $"{repoUrl}/-/merge_requests/{mergeRequestId}";
        // todo: add other source
        return null;
    }

    private string GetCommitUrl(SourceType sourceType, string repoUrl, string commitHash)
    {
        if (sourceType == SourceType.GitLab) return $"{repoUrl}/-/commit/{commitHash}";
        // todo: add other source
        return repoUrl;
    }

    private bool IsBlock(Guid projectId, CiUploadFindingResponse response, ScannerType scannerType)
    {
        var setting = context.ProjectSettings.First(record => record.ProjectId == projectId);
        var projectSetting = JSONSerializer.Deserialize<ProjectSettingMetadata>(setting.Metadata);
        if (projectSetting == null) return false;
        ThresholdMode mode;
        SeverityThreshold? threshold;
        if (scannerManager.IsScaScanner(scannerType))
        {
            if (projectSetting.ScaSetting == null)
            {
                return false;
            }

            mode = projectSetting.ScaSetting.Mode;
            threshold = projectSetting.ScaSetting.SeverityThreshold;
        }
        else
        {
            if (projectSetting.SastSetting == null)
            {
                return false;
            }

            mode = projectSetting.SastSetting.Mode;
            threshold = projectSetting.SastSetting.SeverityThreshold;
        }

        if (threshold == null)
        {
            return false;
        }

        if (mode == ThresholdMode.MonitorOnly)
        {
            return false;
        }

        List<CiFinding> findings = new();
        if (mode == ThresholdMode.BlockOnConfirmation)
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