using CodeSecure.Core.Entity;
using FluentResults;

namespace CodeSecure.Application.Module.Rule;

public static class RuleExtension
{
    public static async Task<Result<Rules>> CreateRuleAsync(this AppDbContext context, Rules request)
    {
        try
        {
            if (!context.Rules.Any(rule => rule.Id == request.Id && rule.ScannerId == request.ScannerId))
            {
                request.CreatedAt = DateTime.UtcNow;
                context.Rules.Add(request);
                await context.SaveChangesAsync();
                return request;
            }

            return Result.Fail("Duplicated rule");
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}