using CodeSecure.Api.Rule.Model;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Manager.Rule;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Rule.Service;

public class RuleService(AppDbContext context, IRuleManager ruleManager) : IRuleService
{
    private readonly FindingStatus[] correctStatus = [FindingStatus.Confirmed, FindingStatus.AcceptedRisk, FindingStatus.Fixed];
    public Task<List<string>> GetRuleIdAsync(RuleFilter filter)
    {
        var query = context.Findings
            .Where(finding => finding.RuleId != null);
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(finding => finding.RuleId!.Contains(finding.Name));
        }

        if (filter.ProjectId != null)
        {
            query = query.Where(finding => finding.ProjectId == filter.ProjectId);
        }

        return query
            .GroupBy(finding => finding.RuleId)
            .Select(finding => finding.Key!).ToListAsync();
    }

    public async Task<Page<RuleInfo>> GetRuleInfoAsync(RuleFilter filter)
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

        return await query.Distinct().Select(rule => new RuleInfo
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

    public async Task UpdateRuleAsync(UpdateRuleRequest request)
    {
        var rule = await context.Rules
            .FirstOrDefaultAsync(rule =>
                rule.Id == request.RuleId &&
                rule.ScannerId == request.ScannerId
            );
        if (rule == null)
        {
            throw new BadRequestException("Rule not found");
        }

        if (request.Confidence != null)
        {
            rule.Confidence = (RuleConfidence)request.Confidence;
        }

        if (request.Status != null)
        {
            rule.Status = (RuleStatus)request.Status;
        }

        context.Rules.Update(rule);
        await context.SaveChangesAsync();
    }

    public async Task<List<Scanners>> GetScannerAsync()
    {
        var scanners = await context.Rules.GroupBy(rule => rule.ScannerId).Select(group => group.Key).ToListAsync();
        return context.Scanners.Where(scanner => scanners.Contains(scanner.Id)).ToList();
    }

    public async Task SyncRules()
    {
        var rules = await context.Findings
            .Where(finding =>
                finding.RuleId != null &&
                context.Rules.Any(rule => rule.Id == finding.RuleId) == false
            )
            .GroupBy(finding => new { finding.RuleId, finding.ScannerId })
            .Select(group => new Rules
            {
                Id = group.Key.RuleId!,
                Status = RuleStatus.Enable,
                Confidence = RuleConfidence.Unknown,
                ScannerId = group.Key.ScannerId,
            }).ToListAsync();
        foreach (var rule in rules)
        {
            await ruleManager.CreateAsync(rule);
        }
        // update confidence
        var rulesInfo = await context.Rules.Where(rule => rule.Status == RuleStatus.Enable).Select(rule => new RuleInfo
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
        }).ToListAsync();
        foreach (var rule in rulesInfo)
        {
            int totalFinding = rule.CorrectFinding + rule.IncorrectFinding + rule.UncertainFinding;
            var rate = (float)rule.CorrectFinding / totalFinding;
            RuleConfidence confidence;
            if (rate >= 0.7)
            {
                confidence = RuleConfidence.High;
            } else if (rate >= 0.3 && rate < 0.7)
            {
                confidence = RuleConfidence.Medium;
            }
            else if (rate > 0)
            {
                confidence = RuleConfidence.Low;
            }
            else
            {
                confidence = RuleConfidence.Unknown;
            }
            await context.Rules.Where(record => record.Id == rule.Id && record.ScannerId == rule.ScannerId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(column => column.Confidence, confidence));
        }
    }
}