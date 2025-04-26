using CodeSecure.Core.Entity;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class UpdateRecommendationPackageCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Packages package)
    {
        try
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