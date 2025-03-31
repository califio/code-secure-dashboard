using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule;

public interface IQueryRuleInfoHandler : IHandler<RuleFilter, Page<RuleInfo>>;
public class QueryRuleInfoHandler(AppDbContext context): IQueryRuleInfoHandler
{
    private readonly FindingStatus[] correctStatus = [FindingStatus.Confirmed, FindingStatus.AcceptedRisk, FindingStatus.Fixed];
    public async Task<Result<Page<RuleInfo>>> HandleAsync(RuleFilter request)
    {
        var query = context.Rules.Include(rule => rule.Scanner!).AsQueryable();
        if (request.ProjectId != null)
        {
            query = query.Where(rule => context.Findings.Any(finding =>
                finding.RuleId == rule.Id &&
                finding.ProjectId == request.ProjectId)
            );
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(rule => rule.Id.Contains(request.Name!));
        }

        if (request.Status is { Count: > 0 })
        {
            query = query.Where(rule => request.Status.Contains(rule.Status));
        }

        if (request.Confidence is { Count: > 0 })
        {
            query = query.Where(rule => request.Confidence.Contains(rule.Confidence));
        }

        if (request.ScannerId is { Count: > 0 })
        {
            query = query.Where(rule => request.ScannerId.Contains(rule.ScannerId));
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
        }).PageAsync(request.Page, request.Size);
    }
}