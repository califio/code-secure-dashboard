using CodeSecure.Application.Module.Package.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class GetPackageByIdCommand(AppDbContext context)
{
    public async Task<Result<PackageDetail>> ExecuteAsync(Guid packageId)
    {
        var package = await context.Packages.FirstOrDefaultAsync(package => package.Id == packageId);
        if (package == null) return Result.Fail("Package not found");

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