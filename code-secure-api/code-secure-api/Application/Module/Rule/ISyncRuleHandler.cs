using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule;

public interface ISyncRuleHandler : IOutputHandler<bool>;
public class SyncRuleHandler(AppDbContext context, ICreateRuleHandler createRuleHandler): ISyncRuleHandler
{
    private readonly FindingStatus[] correctStatus = [FindingStatus.Confirmed, FindingStatus.AcceptedRisk, FindingStatus.Fixed];
    public async Task<Result<bool>> HandleAsync()
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
            await createRuleHandler.HandleAsync(new CreateRuleRequest
            {
                Id = rule.Id,
                ScannerId = rule.ScannerId
            });
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
        return true;
    }
}