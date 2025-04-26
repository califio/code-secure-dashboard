using CodeSecure.Application.Module.Rule.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.Rule.Command;

public class CreateRuleCommand(AppDbContext context)
{
    public async Task<Result<Rules>> ExecuteAsync(CreateRuleRequest request)
    {
        try
        {
            if (context.Rules.Any(record => record.Id == request.Id && record.ScannerId == request.ScannerId) == false)
            {
                var rule = new Rules
                {
                    Id = request.Id,
                    Status = RuleStatus.Enable,
                    Confidence = RuleConfidence.Unknown,
                    ScannerId = request.ScannerId,
                };
                context.Rules.Add(rule);
                await context.SaveChangesAsync();
                return Result.Ok(rule);
            }
            return Result.Fail("Duplicated rule");
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}