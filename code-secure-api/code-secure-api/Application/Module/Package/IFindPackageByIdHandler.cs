using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package;

public interface IFindPackageByIdHandler : IHandler<Guid, PackageDetail>;

public class FindPackageByIdHandler(AppDbContext context) : IFindPackageByIdHandler
{
    public async Task<Result<PackageDetail>> HandleAsync(Guid request)
    {
        var package = await context.Packages.FirstOrDefaultAsync(package => package.Id == request);
        if (package == null) return Result.Fail("Package not found");

        var dependencies = context.PackageDependencies
            .Include(record => record.Dependency!)
            .Where(record => record.PackageId == request)
            .Select(record => record.Dependency!)
            .OrderByDescending(record => record.RiskLevel)
            .ToList();
        var vulnerabilities = context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == request)
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