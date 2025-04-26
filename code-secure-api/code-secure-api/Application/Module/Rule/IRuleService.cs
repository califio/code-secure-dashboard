using CodeSecure.Application.Module.Rule.Command;
using CodeSecure.Application.Module.Rule.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Rule;

public interface IRuleService
{
    Task<Rules> CreateRuleAsync(CreateRuleRequest request);
    Task<Page<RuleInfo>> GetRuleByFilterAsync(RuleFilter filter);
    Task<List<string>> ListRuleIdAsync(RuleFilter filter);
    Task<List<Scanners>> ListScannersAsync();
    Task<bool> SyncRuleAsync();
    Task<Rules> UpdateRuleAsync(UpdateRuleRequest request);
}

public class RuleService(AppDbContext context): IRuleService
{
    public async Task<Rules> CreateRuleAsync(CreateRuleRequest request)
    {
        return (await new CreateRuleCommand(context).ExecuteAsync(request)).GetResult();
    }

    public async Task<Page<RuleInfo>> GetRuleByFilterAsync(RuleFilter filter)
    {
        return (await new GetRuleByFilterCommand(context).ExecuteAsync(filter)).GetResult();
    }

    public async Task<List<string>> ListRuleIdAsync(RuleFilter filter)
    {
        return (await new ListRuleIdCommand(context).ExecuteAsync(filter)).GetResult();
    }

    public async Task<List<Scanners>> ListScannersAsync()
    {
        return (await new ListRuleScannerCommand(context).ExecuteAsync()).GetResult();
    }

    public async Task<bool> SyncRuleAsync()
    {
        return (await new SyncRuleCommand(context).ExecuteAsync()).GetResult();
    }

    public async Task<Rules> UpdateRuleAsync(UpdateRuleRequest request)
    {
        return (await new UpdateRuleCommand(context).ExecuteAsync(request)).GetResult();
    }
}