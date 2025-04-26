using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Ci.Model;
using CodeSecure.Application.Module.Package;
using CodeSecure.Application.Module.Package.Command;
using CodeSecure.Application.Module.Project;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Ci.Command;

public class PushCiDependencyCommand(AppDbContext context)
{
    public async Task<ScanDependencyResult> ExecuteAsync(CiUploadDependencyRequest request)
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
                    await new AddPackageDependenciesCommand(context).ExecuteAsync(record.PkgId, record.Dependencies);
                }
            }
        }

        // add or update vulnerability
        if (request.Vulnerabilities != null)
        {
            foreach (var ciVulnerability in request.Vulnerabilities)
            {
                await new AddPackageVulnerabilitiesCommand(context).ExecuteAsync(ciVulnerability.PkgId,
                    new Vulnerabilities
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
            await new UpdateRiskLevelPackageCommand(context).ExecuteAsync(package);
        }

        foreach (var package in packages)
        {
            await new UpdateRiskImpactPackageCommand(context).ExecuteAsync(package);
            await new UpdateRecommendationPackageCommand(context).ExecuteAsync(package);
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
                    //todo: log exception
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
                //todo: log exception
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
}