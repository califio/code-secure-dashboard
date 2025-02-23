using CodeSecure.Api.Rule.Model;

namespace CodeSecure.Api.Rule.Service;

public interface IRuleService
{
    Task<List<string>> GetRuleIdAsync(RuleFilter filter);
}