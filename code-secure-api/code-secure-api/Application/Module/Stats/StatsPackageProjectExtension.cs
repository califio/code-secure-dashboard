using CodeSecure.Application.Module.Stats.Model;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Stats;
public static class StatsPackageProjectExtension
{
   public static async Task<SeveritySeries> StatsPackageProjectBySeverityAsync(this AppDbContext context, StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new SeveritySeries
        {
            Critical = await context.CountPackageProjectBySeverityAsync(RiskLevel.Critical, filter),
            High = await context.CountPackageProjectBySeverityAsync(RiskLevel.High, filter),
            Medium = await context.CountPackageProjectBySeverityAsync(RiskLevel.Medium, filter),
            Low = await context.CountPackageProjectBySeverityAsync(RiskLevel.Low, filter),
        };
    }

    public static async Task<ScaStatus> StatsPackageProjectByStatusAsync(this AppDbContext context, StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new ScaStatus
        {
            Open = await context.CountPackageProjectByStatusAsync(PackageStatus.Open, filter),
            Ignore = await context.CountPackageProjectByStatusAsync(PackageStatus.Ignore, filter),
            Fixed = await context.CountPackageProjectByStatusAsync(PackageStatus.Fixed, filter),
        };
    }

    public static async Task<List<TopDependency>> StatsTopDependenciesAsync(this AppDbContext context, StatisticFilter filter, int top = 10)
    {
        var topVulnerablePackages = await context.PackageVulnerabilities
            .Where(record => context.ProjectPackages.Any(temp => 
                    temp.PackageId == record.PackageId && 
                    (filter.SourceId == null || temp.Project!.SourceControlId == filter.SourceId) && 
                    (filter.StartDate == null || temp.CreatedAt > filter.StartDate) &&
                    (filter.EndDate == null || temp.CreatedAt < filter.EndDate))
            ).Distinct()
            .GroupBy(record => record.PackageId)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Take(top)
            .ToListAsync();
        var packages = await context.Packages
            .Where(package => topVulnerablePackages.Contains(package.Id))
            .Select(package => new TopDependency
            {
                Group = package.Group,
                Name = package.Name,
                Version = package.Version,
                Critical = context.PackageVulnerabilities.Count(pv =>
                    pv.PackageId == package.Id && pv.Vulnerability!.Severity == FindingSeverity.Critical),
                High = context.PackageVulnerabilities.Count(pv =>
                    pv.PackageId == package.Id && pv.Vulnerability!.Severity == FindingSeverity.High),
                Medium = context.PackageVulnerabilities.Count(pv =>
                    pv.PackageId == package.Id && pv.Vulnerability!.Severity == FindingSeverity.Medium),
                Low = context.PackageVulnerabilities.Count(pv =>
                    pv.PackageId == package.Id && pv.Vulnerability!.Severity == FindingSeverity.Low),
                Info = context.PackageVulnerabilities.Count(pv =>
                    pv.PackageId == package.Id && pv.Vulnerability!.Severity == FindingSeverity.Info)
            }).ToListAsync();
        packages.Sort((package1, package2) =>
            package2.Critical + package2.High + package2.Medium + package2.Low -
            (package1.Critical + package1.High + package1.Medium + package1.Low));
        return packages;
    }
}