using CodeSecure.Core.Entity;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class AddPackageDependenciesCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(string pkgId, IEnumerable<string> pkgDependencies)
    {
        var result = await context.FindPackageByPkgIdAsync(pkgId);
        if (result.IsFailed)
        {
            return Result.Fail(result.ListErrors());
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
                return Result.Fail(e.Message);
            }
        }

        return true;
    }
}