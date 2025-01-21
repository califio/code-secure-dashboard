using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Extension;
using CodeSecure.Manager.Package.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CodeSecure.Manager.Package;

public class PackageManager(
    AppDbContext context,
    IMemoryCache cache,
    ILogger<PackageManager> logger) : IPackageManager
{
    private static readonly SemaphoreSlim Lock = new(1, 1);
    private const int ExpiredTime = 5;

    public async Task<Packages> CreateOrUpdateAsync(Packages package)
    {
        await Lock.WaitAsync();
        try
        {
            var pkg = await FindByPkgIdAsync(package.PkgId);
            if (pkg != null) return pkg;
            package.Id = Guid.NewGuid();
            context.Packages.Add(package);
            await context.SaveChangesAsync();
            CachePackage(package);
            return package;
        }
        finally
        {
            Lock.Release();
        }
    }

    public async Task<Packages?> FindByPkgIdAsync(string pkgId)
    {
        cache.TryGetValue(PackageCacheKey(pkgId), out Packages? package);
        if (package != null) return package;
        package = await context.Packages.FirstOrDefaultAsync(record => record.PkgId == pkgId);
        if (package == null) return package;
        CachePackage(package);
        return package;
    }

    public async Task<List<Packages>> GetDependenciesAsync(string pkgId)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package == null)
        {
            return [];
        }

        return await GetDependenciesAsync(package.Id);
    }

    public async Task<List<Packages>> GetDependenciesAsync(Guid id)
    {
        return await context.PackageDependencies
            .Include(record => record.Dependency)
            .Where(record => record.PackageId == id)
            .Select(record => record.Dependency!)
            .ToListAsync();
    }

    public async Task AddDependenciesAsync(string pkgId, IEnumerable<string> pkgDependencies)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package == null)
        {
            return;
        }

        Dictionary<string, Packages> mDependencies = new();
        var dependencies = await GetDependenciesAsync(package.Id);
        foreach (var dependency in dependencies)
        {
            mDependencies[dependency.PkgId] = dependency;
        }

        foreach (var dependencyPkgId in pkgDependencies)
        {
            if (!mDependencies.ContainsKey(dependencyPkgId))
            {
                var dependency = await FindByPkgIdAsync(dependencyPkgId);
                if (dependency != null)
                {
                    context.PackageDependencies.Add(new PackageDependencies
                    {
                        PackageId = package.Id,
                        DependencyId = dependency.Id,
                    });
                    await context.SaveChangesAsync();
                    mDependencies[dependency.PkgId] = dependency;
                }
            }
        }
    }

    public async Task<List<Vulnerabilities>> GetVulnerabilitiesAsync(string pkgId)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package == null)
        {
            return [];
        }

        return await GetVulnerabilitiesAsync(package.Id);
    }

    public async Task<List<Vulnerabilities>> GetVulnerabilitiesAsync(Guid packageId)
    {
        return await context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == packageId)
            .Select(record => record.Vulnerability!)
            .ToListAsync();
    }

    public async Task AddVulnerabilityAsync(string pkgId, Vulnerabilities vulnerability)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package != null)
        {
            await AddVulnerabilityAsync(package, vulnerability);
        }
    }

    public async Task AddVulnerabilityAsync(Packages package, Vulnerabilities vulnerability)
    {
        var issue = await FindVulnerabilityByIdentityAsync(vulnerability.Identity);
        if (issue == null)
        {
            vulnerability.Id = Guid.NewGuid();
            context.Vulnerabilities.Add(vulnerability);
            await context.SaveChangesAsync();
            issue = vulnerability;
            CacheVulnerability(issue);
        }

        if (!context.PackageVulnerabilities.Any(record =>
                record.PackageId == package.Id && record.VulnerabilityId == issue.Id))
        {
            context.PackageVulnerabilities.Add(new PackageVulnerabilities
            {
                PackageId = package.Id,
                VulnerabilityId = issue.Id,
            });
            await context.SaveChangesAsync();
        }
    }

    public async Task AddToProjectAsync(Guid projectId, Packages package, string location)
    {
        var project = await FindProjectByIdAsync(projectId);
        if (project == null)
        {
            logger.LogWarning($"Not found project by id {projectId.ToString()}");
            return;
        }

        var exists = await context.ProjectPackages.AnyAsync(record =>
            record.ProjectId == projectId &&
            record.PackageId == package.Id &&
            record.Location == location);
        if (!exists)
        {
            context.ProjectPackages.Add(new ProjectPackages
            {
                ProjectId = projectId,
                PackageId = package.Id,
                Location = location,
            });
            await context.SaveChangesAsync();
        }
    }

    public async Task AddToProjectAsync(Guid projectId, string pkgId, string location)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package == null)
        {
            logger.LogWarning($"Not found package by pkgId {pkgId}");
            return;
        }
        await AddToProjectAsync(projectId, package, location);
    }

    public async Task RemoveFromProjectAsync(Guid projectId, Guid packageId, string location)
    {
        var packageProject = await context.ProjectPackages.FirstOrDefaultAsync(record =>
            record.ProjectId == projectId && 
            record.PackageId == packageId && 
            record.Location == location);
        if (packageProject != null)
        {
            context.ProjectPackages.Remove(packageProject);
            await context.SaveChangesAsync();
        }
    }

    public async Task RemoveFromProjectAsync(Guid projectId, string pkgId, string location)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package != null)
        {
            await RemoveFromProjectAsync(projectId, package.Id, location);
        }
    }

    public async Task<List<PackageProject>> FromProjectAsync(Guid projectId)
    {
        return await context.ProjectPackages
            .Include(record => record.Package)
            .Where(record => record.ProjectId == projectId)
            .Select(record => new PackageProject
            {
                Package = record.Package!,
                Location = record.Location,
            })
            .ToListAsync();
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
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
            }
            
        }
    }

    public async Task UpdateRiskLevelAsync(string pkgId)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package != null)
        {
            await UpdateRiskImpactAsync(package);
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
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }

    public async Task UpdateRiskImpactAsync(string pkgId)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package != null)
        {
            await UpdateRiskImpactAsync(package);
        }
    }

    public async Task UpdateRecommendationAsync(string pkgId)
    {
        var package = await FindByPkgIdAsync(pkgId);
        if (package != null)
        {
            await UpdateRecommendationAsync(package);
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
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }

    private void CachePackage(Packages package)
    {
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredTime));
        cache.Set(PackageCacheKey(package), package, options);
    }

    private void CacheVulnerability(Vulnerabilities vulnerability)
    {
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredTime));
        cache.Set(VulnerabilityCacheKey(vulnerability), vulnerability, options);
    }

    private void CacheProject(Projects project)
    {
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredTime));
        cache.Set(ProjectCacheKey(project.Id), project, options);
    }

    private async Task<Vulnerabilities?> FindVulnerabilityByIdentityAsync(string identity)
    {
        cache.TryGetValue(VulnerabilityCacheKey(identity), out Vulnerabilities? vulnerability);
        if (vulnerability != null) return vulnerability;
        vulnerability = await context.Vulnerabilities.FirstOrDefaultAsync(record => record.Identity == identity);
        if (vulnerability == null) return vulnerability;
        CacheVulnerability(vulnerability);
        return vulnerability;
    }

    private async Task<Projects?> FindProjectByIdAsync(Guid projectId)
    {
        cache.TryGetValue(ProjectCacheKey(projectId), out Projects? project);
        if (project != null) return project;
        project = await context.Projects.FirstOrDefaultAsync(record => record.Id == projectId);
        if (project == null) return project;
        CacheProject(project);
        return project;
    }


    private string PackageCacheKey(Packages package)
    {
        return PackageCacheKey(package.PkgId);
    }

    private string PackageCacheKey(string pkgId)
    {
        return $"package:{pkgId}";
    }

    private string VulnerabilityCacheKey(Vulnerabilities vulnerability)
    {
        return VulnerabilityCacheKey(vulnerability.Identity);
    }

    private string VulnerabilityCacheKey(string identity)
    {
        return $"vulnerability:{identity}";
    }

    private string ProjectCacheKey(Guid projectId)
    {
        return $"project:{projectId.ToString()}";
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