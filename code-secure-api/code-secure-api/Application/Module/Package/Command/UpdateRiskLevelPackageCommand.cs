using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class UpdateRiskLevelPackageCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Packages package)
    {
        try
        {
            var hightestVulnerability = await context.PackageVulnerabilities
                .Include(record => record.Vulnerability)
                .Where(record => record.PackageId == package.Id)
                .OrderByDescending(record => record.Vulnerability!.Severity)
                .Select(record => record.Vulnerability)
                .FirstOrDefaultAsync();
            if (hightestVulnerability != null)
            {
                var riskLevel = FromFindingSeverity(hightestVulnerability.Severity);
                var tracked = context.ChangeTracker.Entries<Packages>()
                    .FirstOrDefault(e => e.Entity.Id == package.Id);
                if (tracked != null)
                {
                    tracked.Entity.RiskImpact = RiskImpact.Direct;
                    tracked.Entity.RiskLevel = riskLevel;
                }
                else
                {
                    package.RiskImpact = RiskImpact.Direct;
                    package.RiskLevel = riskLevel;
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
    private RiskLevel FromFindingSeverity(FindingSeverity severity)
    {
        if (severity == FindingSeverity.Critical) return RiskLevel.Critical;
        if (severity == FindingSeverity.High) return RiskLevel.High;
        if (severity == FindingSeverity.Medium) return RiskLevel.Medium;
        if (severity == FindingSeverity.Low) return RiskLevel.Low;
        return RiskLevel.None;
    }
}