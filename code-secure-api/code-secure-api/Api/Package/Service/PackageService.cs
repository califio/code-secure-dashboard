using CodeSecure.Api.Package.Model;
using CodeSecure.Database;
using CodeSecure.Exception;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Package.Service;

public class PackageService(AppDbContext context) : IPackageService
{
    public async Task<List<PackageInfo>> GetPackageDependenciesAsync(Guid packageId)
    {
        if (await context.Packages.AnyAsync(package => package.Id == packageId) == false)
            throw new BadRequestException("package not found");
        return await context.PackageDependencies.Include(record => record.Dependency)
            .Where(record => record.PackageId == packageId)
            .Select(record => new PackageInfo
            {
                Id = record.DependencyId,
                Group = record.Dependency!.Group,
                Name = record.Dependency!.Name,
                Version = record.Dependency!.Version,
                Type = record.Dependency!.Type,
                DependencyCount = context.PackageDependencies.Count(p => p.PackageId == record.DependencyId),
                IsVulnerable = context.PackageVulnerabilities.Any(p => p.PackageId == record.DependencyId)
            }).ToListAsync();
    }

    public async Task<PackageDetail> GetPackageByIdAsync(Guid packageId)
    {
        var package = await context.Packages.FirstOrDefaultAsync(package => package.Id == packageId);
        if (package == null)
        {
            throw new BadRequestException("Package not found");
        }

        var dependencies = context.PackageDependencies
            .Include(record => record.Dependency!)
            .Where(record => record.PackageId == packageId)
            .Select(record => record.Dependency!)
            .OrderByDescending(record => record.RiskLevel)
            .ToList();
        var vulnerabilities = context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == packageId)
            .Select(record => record.Vulnerability!)
            .OrderByDescending(record => record.Severity)
            .ToList();
        return new PackageDetail
        {
            Info = package,
            Vulnerabilities = vulnerabilities,
            Dependencies = dependencies
        };
    }
}