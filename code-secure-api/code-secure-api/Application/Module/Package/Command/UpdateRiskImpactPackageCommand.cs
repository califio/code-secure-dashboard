using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package.Command;

public class UpdateRiskImpactPackageCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Packages package)
    {
        try
        {
            if (package.RiskLevel == RiskLevel.None)
            {
                var hightestRiskDependency = await context.PackageDependencies
                    .Include(record => record.Dependency)
                    .Where(record => record.PackageId == package.Id)
                    .OrderByDescending(record => record.Package!.RiskLevel)
                    .Select(record => record.Package)
                    .FirstOrDefaultAsync();
                var tracked = context.ChangeTracker.Entries<Packages>()
                    .FirstOrDefault(e => e.Entity.Id == package.Id);
                var riskImpact = RiskImpact.None;
                if (hightestRiskDependency != null && hightestRiskDependency.RiskLevel != RiskLevel.None)
                {
                    riskImpact = RiskImpact.Indirect;
                }

                if (tracked != null)
                {
                    tracked.Entity.RiskImpact = riskImpact;
                }
                else
                {
                    package.RiskImpact = riskImpact;
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