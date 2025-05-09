using CodeSecure.Application;
using CodeSecure.Application.Module.Package;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.CI;

public interface IPackageManager
{
    Task AddDependenciesAsync(string pkgId, IEnumerable<string> pkgDependencies);
    Task AddVulnerabilityAsync(string pkgId, Vulnerabilities vulnerability);
    Task UpdateRiskLevelAsync(Packages package);
    Task UpdateRiskImpactAsync(Packages package);
    Task UpdateRecommendationAsync(Packages package);
}

public class PackageManager(
    AppDbContext context,
    ILogger<PackageManager> logger) : IPackageManager
{
    public async Task AddDependenciesAsync(string pkgId, IEnumerable<string> pkgDependencies)
    {
        var result = await context.FindPackageByPkgIdAsync(pkgId);
        if (result.IsFailed)
        {
            return;
        }

        var package = result.Value;
        var dependencies = context.PackageDependencies
            .Include(record => record.Dependency)
            .Where(record => record.PackageId == package.Id)
            .Select(record => record.Dependency!.PkgId)
            .ToHashSet();
        foreach (var dependencyPkgId in pkgDependencies)
        {
            if (dependencies.Contains(dependencyPkgId)) continue;
            result = await context.FindPackageByPkgIdAsync(dependencyPkgId);
            if (result.IsFailed) continue;
            var dependency = result.Value;
            try
            {
                context.PackageDependencies.Add(new PackageDependencies
                {
                    PackageId = package.Id,
                    DependencyId = dependency.Id,
                });
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }


    public async Task AddVulnerabilityAsync(string pkgId, Vulnerabilities vulnerability)
    {
        var result = await context.FindPackageByPkgIdAsync(pkgId);
        if (result.IsSuccess)
        {
            try
            {
                await AddVulnerabilityAsync(result.Value, vulnerability);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }

    private async Task AddVulnerabilityAsync(Packages package, Vulnerabilities vulnerability)
    {
        var issue = await context.Vulnerabilities.FirstOrDefaultAsync(x => x.Identity == vulnerability.Identity);
        if (issue == null)
        {
            vulnerability.Id = Guid.NewGuid();
            context.Vulnerabilities.Add(vulnerability);
            await context.SaveChangesAsync();
            issue = vulnerability;
        }

        if (!context.PackageVulnerabilities.Any(record =>
                record.PackageId == package.Id &&
                record.VulnerabilityId == issue.Id
            ))
        {
            context.PackageVulnerabilities.Add(new PackageVulnerabilities
            {
                PackageId = package.Id,
                VulnerabilityId = issue.Id,
            });
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateRiskLevelAsync(Packages package)
    {
        var hightestVulnerability = await context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == package.Id)
            .OrderByDescending(record => record.Vulnerability!.Severity)
            .Select(record => record.Vulnerability)
            .FirstOrDefaultAsync();
        if (hightestVulnerability != null)
        {
            var riskLevel = FromFindingSeverity(hightestVulnerability.Severity);
            var tracked = context.ChangeTracker.Entries<Packages>()
                .FirstOrDefault(e => e.Entity.Id == package.Id);
            if (tracked != null)
            {
                tracked.Entity.RiskImpact = RiskImpact.Direct;
                tracked.Entity.RiskLevel = riskLevel;
            }
            else
            {
                package.RiskImpact = RiskImpact.Direct;
                package.RiskLevel = riskLevel;
                context.Packages.Update(package);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }

    public async Task UpdateRiskImpactAsync(Packages package)
    {
        if (package.RiskLevel == RiskLevel.None)
        {
            var hightestRiskDependency = await context.PackageDependencies
                .Include(record => record.Dependency)
                .Where(record => record.PackageId == package.Id)
                .OrderByDescending(record => record.Package!.RiskLevel)
                .Select(record => record.Package)
                .FirstOrDefaultAsync();
            var tracked = context.ChangeTracker.Entries<Packages>()
                .FirstOrDefault(e => e.Entity.Id == package.Id);
            var riskImpact = RiskImpact.None;
            if (hightestRiskDependency != null && hightestRiskDependency.RiskLevel != RiskLevel.None)
            {
                riskImpact = RiskImpact.Indirect;
            }

            if (tracked != null)
            {
                tracked.Entity.RiskImpact = riskImpact;
            }
            else
            {
                package.RiskImpact = riskImpact;
                context.Packages.Update(package);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }


    public async Task UpdateRecommendationAsync(Packages package)
    {
        var fixedVersions = await context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == package.Id)
            .Select(record => record.Vulnerability!.FixedVersion)
            .ToListAsync();
        List<VersionInfo> versions = new();
        foreach (var fixedVersion in fixedVersions)
        {
            foreach (var version in fixedVersion.Split(","))
            {
                if (string.IsNullOrEmpty(version.Trim())) continue;
                if (!VersionInfo.TryParse(version.Trim(), out VersionInfo? v)) continue;
                if (v != null) versions.Add(v);
            }
        }

        versions.Sort((v1, v2) => v2.CompareTo(v1));
        if (versions.Count > 0)
        {
            var tracked = context.ChangeTracker.Entries<Packages>()
                .FirstOrDefault(e => e.Entity.Id == package.Id);
            if (tracked != null)
            {
                tracked.Entity.FixedVersion = versions[0].ToString();
            }
            else
            {
                package.FixedVersion = versions[0].ToString();
                context.Packages.Update(package);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }

    private RiskLevel FromFindingSeverity(FindingSeverity severity)
    {
        if (severity == FindingSeverity.Critical) return RiskLevel.Critical;
        if (severity == FindingSeverity.High) return RiskLevel.High;
        if (severity == FindingSeverity.Medium) return RiskLevel.Medium;
        if (severity == FindingSeverity.Low) return RiskLevel.Low;
        return RiskLevel.None;
    }
}