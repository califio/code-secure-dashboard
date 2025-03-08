using CodeSecure.Database;
using CodeSecure.Enum;
using CodeSecure.Manager.Scanner;
using CodeSecure.Manager.Statistic.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Statistic;

public class StatisticManager(AppDbContext context, IScannerManager scannerManager) : IStatisticManager
{
    public async Task<SeveritySeries> SeveritySastAsync(StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new SeveritySeries
        {
            Critical = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Severity == FindingSeverity.Critical &&
                finding.Status != FindingStatus.Incorrect &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            High = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Severity == FindingSeverity.High &&
                finding.Status != FindingStatus.Incorrect &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Medium = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Severity == FindingSeverity.Medium &&
                finding.Status != FindingStatus.Incorrect &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Low = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Severity == FindingSeverity.Low &&
                finding.Status != FindingStatus.Incorrect &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
        };
    }

    public async Task<SeveritySeries> SeverityScaAsync(StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new SeveritySeries
        {
            Critical = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel == RiskLevel.Critical &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            High = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel == RiskLevel.High &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Medium = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel == RiskLevel.Medium &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Low = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel == RiskLevel.Low &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
        };

    }

    public async Task<SastStatus> StatusSastAsync(StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new SastStatus
        {
            Open = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Status == FindingStatus.Open &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Confirmed = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Status == FindingStatus.Confirmed &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            AcceptedRisk = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Status == FindingStatus.AcceptedRisk &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Fixed = await context.Findings.CountAsync(finding =>
                (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
                finding.Status == FindingStatus.Fixed &&
                (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId))
            ),
        };
    }

    public async Task<ScaStatus> StatusScaAsync(StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        
        return new ScaStatus
        {
            Open = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel != RiskLevel.None &&
                package.Status == PackageStatus.Open &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Ignore = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel != RiskLevel.None &&
                package.Status == PackageStatus.Ignore &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
            Fixed = await context.ProjectPackages.CountAsync(package =>
                (filter.ProjectId == null || package.ProjectId == filter.ProjectId) &&
                package.Package!.RiskLevel != RiskLevel.None &&
                package.Status == PackageStatus.Fixed &&
                (package.CreatedAt >= filter.StartDate && package.CreatedAt < filter.EndDate) &&
                (filter.SourceId == null || context.Projects.Any(record => record.Id == package.ProjectId && record.SourceControlId == filter.SourceId))
            ),
        };
    }

    public async Task<List<TopFinding>> TopSastFindingAsync(StatisticFilter filter, int top = 10)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        var categories = new Dictionary<string, int> { { "Other", 0 } };
        var scanners = (await scannerManager.GetSastScannersAsync())
            .Select(item => item.Id).ToList();
        var query = context.Findings.Where(finding =>
            (filter.ProjectId == null || finding.ProjectId == filter.ProjectId) &&
            finding.Status != FindingStatus.Incorrect &&
            scanners.Contains(finding.ScannerId) && 
            (finding.CreatedAt >= filter.StartDate && finding.CreatedAt < filter.EndDate) &&
            (filter.SourceId == null || context.Projects.Any(record => 
                record.Id == finding.ProjectId && record.SourceControlId == filter.SourceId)
            )
        );
        //severity
        query.GroupBy(finding => finding.Category).Select(g => new
        {
            Category = g.Key,
            Count = g.Count()
        }).ToList().ForEach(entry =>
        {
            if (string.IsNullOrEmpty(entry.Category) || entry.Category == "Other")
                categories["Other"] += entry.Count;
            else
                categories.TryAdd(entry.Category, entry.Count);
        });
        if (categories["Other"] == 0) categories.Remove("Other");
        var sortedCategories = categories.OrderByDescending(pair => pair.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        var result = new List<TopFinding>();

        foreach (var item in sortedCategories)
            if (result.Count < top)
                result.Add(new TopFinding
                {
                    Category = item.Key,
                    Count = item.Value
                });
            else
                break;

        return result;
    }

    public async Task<List<TopDependency>> TopDependenciesAsync(StatisticFilter filter, int top = 10)
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