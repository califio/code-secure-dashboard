using CodeSecure.Application.Module.Stats.Model;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Stats;

public static class StatisticExtension
{
    public static async Task<int> CountSastFindingBySeverityAsync(this AppDbContext context, FindingSeverity severity,
        StatisticFilter filter)
    {
        return await context.Findings.CountAsync(finding =>
            (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
            finding.Severity == severity &&
            finding.Status != FindingStatus.Incorrect &&
            (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
            (filter.SourceId == null || context.Projects.Any(record =>
                record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
        );
    }

    public static async Task<int> CountSastFindingByStatusAsync(this AppDbContext context, FindingStatus status,
        StatisticFilter filter)
    {
        return await context.Findings.CountAsync(finding =>
            (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
            finding.Status == status &&
            (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
            (filter.SourceId == null || context.Projects.Any(record =>
                record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
        );
    }

    public static async Task<int> CountPackageProjectBySeverityAsync(this AppDbContext context, RiskLevel severity,
        StatisticFilter filter)
    {
        return await context.ProjectPackages.CountAsync(package =>
            (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
            package.Package!.RiskLevel == severity &&
            (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
            (filter.SourceId == null || context.Projects.Any(record =>
                record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
        );
    }

    public static async Task<int> CountPackageProjectByStatusAsync(this AppDbContext context, PackageStatus status,
        StatisticFilter filter)
    {
        return await context.ProjectPackages.CountAsync(package =>
            (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
            package.Package!.RiskLevel != RiskLevel.None &&
            package.Status == status &&
            (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
            (filter.SourceId == null || context.Projects.Any(record =>
                record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
        );
    }
}