using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CodeSecure.Application.Module.Package;

public static class PackageExtension
{
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());
    private const int ExpiredTime = 5; // minus

    public static async Task<Result<Packages>> CreatePackageAsync(this AppDbContext context, Packages package)
    {
        try
        {
            var cachePackage = await context.FindPackageByPkgIdAsync(package.PkgId);
            if (cachePackage.IsSuccess)
            {
                return cachePackage.Value;
            }

            package.Id = Guid.NewGuid();
            context.Packages.Add(package);
            await context.SaveChangesAsync();
            return package;
        }
        catch (Exception e)
        {
            // try get again (in case race condition)
            var cachePackage = await context.FindPackageByPkgIdAsync(package.PkgId);
            if (cachePackage.IsSuccess)
            {
                return cachePackage.Value;
            }

            return Result.Fail(e.Message);
        }
    }

    public static async Task<Result<Packages>> FindPackageByPkgIdAsync(this AppDbContext context, string pkgId)
    {
        Cache.TryGetValue(pkgId, out Packages? package);
        if (package != null) return package;
        package = await context.Packages.FirstOrDefaultAsync(record => record.PkgId == pkgId);
        if (package == null) return Result.Fail("Package not found");
        CachePackageByPkgId(package);
        return package;
    }

    private static void CachePackageByPkgId(Packages package)
    {
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredTime));
        Cache.Set(package.PkgId, package, options);
    }
}