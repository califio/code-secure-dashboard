using CodeSecure.Application.Module.Statistic.Model;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Statistic;

public interface IStatsSastFinding
{
    Task<SeveritySeries> StatsSastFindingBySeverityAsync(StatisticFilter filter);
    Task<SastStatus> StatsSastFindingByStatusAsync(StatisticFilter filter);
    Task<List<TopFinding>> StatsTopSastFindingAsync(StatisticFilter filter, int top = 10);
}

public class StatsSastFinding(AppDbContext context) : IStatsSastFinding
{
    public async Task<SeveritySeries> StatsSastFindingBySeverityAsync(StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new SeveritySeries
        {
            Critical = await context.CountSastFindingBySeverityAsync(FindingSeverity.Critical, filter),
            High = await context.CountSastFindingBySeverityAsync(FindingSeverity.High, filter),
            Medium = await context.CountSastFindingBySeverityAsync(FindingSeverity.Medium, filter),
            Low = await context.CountSastFindingBySeverityAsync(FindingSeverity.Low, filter),
        };
    }
    public async Task<SastStatus> StatsSastFindingByStatusAsync(StatisticFilter filter)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        return new SastStatus
        {
            Open = await context.CountSastFindingByStatusAsync(FindingStatus.Open, filter),
            Confirmed = await context.CountSastFindingByStatusAsync(FindingStatus.Confirmed, filter),
            AcceptedRisk = await context.CountSastFindingByStatusAsync(FindingStatus.AcceptedRisk, filter),
            Fixed = await context.CountSastFindingByStatusAsync(FindingStatus.Fixed, filter),
        };
    }
    public async Task<List<TopFinding>> StatsTopSastFindingAsync(StatisticFilter filter, int top = 10)
    {
        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.UtcNow;
        var categories = new Dictionary<string, int> { { "Other", 0 } };
        var scanners = await context.Scanners
            .Where(scanner => scanner.Type == ScannerType.Sast || scanner.Type == ScannerType.Secret)
            .Select(scanner => scanner.Id)
            .ToListAsync();
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
}