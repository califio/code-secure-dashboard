using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Rule;

public interface IRuleManager
{
    public Task<RuleResult> CreateAsync(Rules rule);
}