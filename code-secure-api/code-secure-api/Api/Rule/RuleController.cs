using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Rule;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Rule;

public class RuleController(
    IListRuleIdHandler listRuleIdHandler,
    IQueryRuleInfoHandler queryRuleInfoHandler,
    IUpdateRuleHandler updateRuleHandler,
    IListScannerRuleHandler listScannerRuleHandler,
    ISyncRuleHandler syncRuleHandler
) : BaseController
{
    [HttpPost]
    [Route("list")]
    public async Task<List<string>> QueryRuleId(RuleFilter filter)
    {
        var result = await listRuleIdHandler.HandleAsync(filter);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.ListErrors());
    }

    [HttpPost]
    [Route("filter")]
    public async Task<Page<RuleInfo>> GetRuleInfo(RuleFilter filter)
    {
        var result = await queryRuleInfoHandler.HandleAsync(filter);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.ListErrors());
    }

    [HttpPost]
    [Route("update")]
    [Permission(PermissionType.Rule, PermissionAction.Update)]
    public async Task UpdateRule(UpdateRuleRequest request)
    {
        var result = await updateRuleHandler.HandleAsync(request);
        if (result.IsFailed)
        {
            throw new BadRequestException(result.ListErrors());
        }
    }

    [HttpGet]
    [Route("scanner")]
    public async Task<List<Scanners>> GetRuleScanners()
    {
        var result = await listScannerRuleHandler.HandleAsync();
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.ListErrors());
    }

    [HttpPost]
    [Route("sync")]
    [Permission(PermissionType.Rule, PermissionAction.Update)]
    public async Task SyncRules()
    {
        var result = await syncRuleHandler.HandleAsync();
        if (result.IsFailed)
        {
            throw new BadRequestException(result.ListErrors());
        }
    }
}