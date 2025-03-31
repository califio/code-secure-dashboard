using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.Rule;

public record CreateRuleRequest
{
    public required string Id { get; init; }
    public required Guid ScannerId { get; init; }
}

public interface ICreateRuleHandler : IHandler<CreateRuleRequest, Rules>;
public class CreateRuleHandler(AppDbContext context): ICreateRuleHandler
{
    public async Task<Result<Rules>> HandleAsync(CreateRuleRequest request)
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