using CodeSecure.Application.Module.Package.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class ListPackageDependencyCommand(AppDbContext context)
{
    public async Task<Result<List<PackageInfo>>> ExecuteAsync(Guid packageId)
    {
        if (await context.Packages.AnyAsync(package => package.Id == packageId) == false)
            return Result.Fail("Package not found");
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