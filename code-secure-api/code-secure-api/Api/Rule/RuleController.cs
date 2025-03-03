using CodeSecure.Api.Rule.Model;
using CodeSecure.Api.Rule.Service;
using CodeSecure.Authentication;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Rule;

public class RuleController(IRuleService ruleService) : BaseController
{
    [HttpPost]
    [Route("list")]
    public Task<List<string>> GetRuleId(RuleFilter filter)
    {
        return ruleService.GetRuleIdAsync(filter);
    }
    
    [HttpPost]
    [Route("filter")]
    public Task<Page<RuleInfo>> GetRuleInfo(RuleFilter filter)
    {
        return ruleService.GetRuleInfoAsync(filter);
    }
    
    [HttpPost]
    [Route("update")]
    public async Task UpdateRule(UpdateRuleRequest request)
    { 
        await ruleService.UpdateRuleAsync(request);
    }
    
    [HttpGet]
    [Route("scanner")]
    public async Task<List<Scanners>> GetRuleScanners()
    { 
       return await ruleService.GetScannerAsync();
    }
    
    [HttpPost]
    [Route("sync")]
    [Permission(PermissionType.Rule, PermissionAction.Update)]
    public Task SyncRules()
    {
        return ruleService.SyncRules();
    }
}