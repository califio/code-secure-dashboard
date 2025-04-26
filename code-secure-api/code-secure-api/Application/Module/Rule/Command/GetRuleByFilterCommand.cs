using CodeSecure.Application.Module.Rule.Model;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule.Command;
public class GetRuleByFilterCommand(AppDbContext context)
{
    private readonly FindingStatus[] correctStatus = [FindingStatus.Confirmed, FindingStatus.AcceptedRisk, FindingStatus.Fixed];
    public async Task<Result<Page<RuleInfo>>> ExecuteAsync(RuleFilter filter)
    {
        var query = context.Rules.Include(rule => rule.Scanner!).AsQueryable();
        if (filter.ProjectId != null)
        {
            query = query.Where(rule => context.Findings.Any(finding =>
                finding.RuleId == rule.Id &&
                finding.ProjectId == filter.ProjectId)
            );
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(rule => rule.Id.Contains(filter.Name!));
        }

        if (filter.Status is { Count: > 0 })
        {
            query = query.Where(rule => filter.Status.Contains(rule.Status));
        }

        if (filter.Confidence is { Count: > 0 })
        {
            query = query.Where(rule => filter.Confidence.Contains(rule.Confidence));
        }

        if (filter.ScannerId is { Count: > 0 })
        {
            query = query.Where(rule => filter.ScannerId.Contains(rule.ScannerId));
        }

        return await query.Distinct().OrderByDescending(rule => rule.CreatedAt).Select(rule => new RuleInfo
        {
            Id = rule.Id,
            Status = rule.Status,
            Confidence = rule.Confidence,
            CreatedAt = rule.CreatedAt,
            UpdatedAt = rule.UpdatedAt,
            ScannerId = rule.ScannerId,
            ScannerName = rule.Scanner!.Name,
            IncorrectFinding = context.Findings.Count(finding =>
                finding.Status == FindingStatus.Incorrect && finding.RuleId == rule.Id),
            CorrectFinding = context.Findings.Count(finding =>
                correctStatus.Contains(finding.Status) && finding.RuleId == rule.Id),
            UncertainFinding =
                context.Findings.Count(finding => finding.Status == FindingStatus.Open && finding.RuleId == rule.Id)
        }).PageAsync(filter.Page, filter.Size);
    }
}