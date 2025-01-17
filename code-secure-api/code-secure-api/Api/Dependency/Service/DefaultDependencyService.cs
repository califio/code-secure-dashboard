using CodeSecure.Api.Dependency.Model;
using CodeSecure.Database;
using CodeSecure.Exception;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Dependency.Service;

public class DefaultDependencyService(AppDbContext context) : IDependencyService
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
}