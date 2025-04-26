using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class AddPackageVulnerabilitiesCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(string pkgId, Vulnerabilities vulnerability)
    {
        var result = await context.FindPackageByPkgIdAsync(pkgId);
        if (result.IsFailed)
        {
            return Result.Fail("Pacakge not found");
        }

        try
        {
            var package = result.Value;
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

            return true;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}