using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule;

public class UpdateRuleRequest
{
    [Required]
    public required string RuleId { get; set; }
    [Required]
    public required Guid ScannerId { get; set; }
    public RuleStatus? Status { get; set; }
    public RuleConfidence? Confidence { get; set; }
}

public interface IUpdateRuleHandler : IHandler<UpdateRuleRequest, Rules>;

public class UpdateRuleHandler(AppDbContext context) : IUpdateRuleHandler
{
    public async Task<Result<Rules>> HandleAsync(UpdateRuleRequest request)
    {
        var rule = await context.Rules
            .FirstOrDefaultAsync(rule =>
                rule.Id == request.RuleId &&
                rule.ScannerId == request.ScannerId
            );
        if (rule == null)
        {
            return Result.Fail("Rule not found");
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
        return rule;
    }
}