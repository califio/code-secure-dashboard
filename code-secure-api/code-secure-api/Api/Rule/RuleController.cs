using CodeSecure.Api.Rule.Model;
using CodeSecure.Api.Rule.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Rule;

public class RuleController(IRuleService ruleService) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public Task<List<string>> GetRules(RuleFilter filter)
    {
        return ruleService.GetRuleIdAsync(filter);
    }
}