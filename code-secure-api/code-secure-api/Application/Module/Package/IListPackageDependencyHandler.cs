using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package;

public interface IListPackageDependencyHandler : IHandler<Guid, List<PackageInfo>>;

public class ListPackageDependencyHandler(AppDbContext context) : IListPackageDependencyHandler
{
    public async Task<Result<List<PackageInfo>>> HandleAsync(Guid request)
    {
        if (await context.Packages.AnyAsync(package => package.Id == request) == false)
            return Result.Fail("Package not found");
        return await context.PackageDependencies.Include(record => record.Dependency)
            .Where(record => record.PackageId == request)
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