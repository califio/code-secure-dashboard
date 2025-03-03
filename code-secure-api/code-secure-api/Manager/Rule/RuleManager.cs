using CodeSecure.Database;
using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Rule;

public class RuleManager(
    AppDbContext context
) : IRuleManager
{
    public async Task<RuleResult> CreateAsync(Rules rule)
    {
        try
        {
            if (!context.Rules.Any(record => record.Id == rule.Id && record.ScannerId == rule.ScannerId))
            {
                rule.CreatedAt = DateTime.UtcNow;
                context.Rules.Add(rule);
                await context.SaveChangesAsync();
                return RuleResult.Success;
            }
            return RuleResult.Failed("Duplicated rule");
        }
        catch (System.Exception e)
        {
            return RuleResult.Failed(e.Message);
        }
        
    }
}