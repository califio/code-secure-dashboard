using CodeSecure.Application.Module.Rule;
using CodeSecure.Application.Module.Rule.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Rule;

public class RuleController(
    IRuleService ruleService
) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public Task<Page<RuleInfo>> GetRuleByFilter(RuleFilter filter)
    {
        return ruleService.GetRuleByFilterAsync(filter);
    }

    [HttpPost]
    [Route("update")]
    [Permission(PermissionType.Rule, PermissionAction.Update)]
    public Task UpdateRule(UpdateRuleRequest request)
    {
        return ruleService.UpdateRuleAsync(request);
    }

    [HttpGet]
    [Route("scanner")]
    public Task<List<Scanners>> GetRuleScanners()
    {
        return ruleService.ListScannersAsync();
    }

    [HttpPost]
    [Route("sync")]
    [Permission(PermissionType.Rule, PermissionAction.Update)]
    public Task<bool> SyncRules()
    {
        return ruleService.SyncRuleAsync();
    }
}