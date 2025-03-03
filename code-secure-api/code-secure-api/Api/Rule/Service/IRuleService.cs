using CodeSecure.Api.Rule.Model;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;

namespace CodeSecure.Api.Rule.Service;

public interface IRuleService
{
    Task<List<string>> GetRuleIdAsync(RuleFilter filter);
    Task<Page<RuleInfo>> GetRuleInfoAsync(RuleFilter filter);
    Task UpdateRuleAsync(UpdateRuleRequest request);
    Task<List<Scanners>> GetScannerAsync();
    Task SyncRules();
}