using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

[Microsoft.AspNetCore.Components.Route("api/integration/teams")]
[ApiExplorerSettings(GroupName = "Integration")]
public class TeamsIntegrationController(ITeamsAlertSettingService teamsAlertSettingService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<TeamsAlertSetting> GetTeamsIntegrationSetting()
    {
        return (await teamsAlertSettingService.GetSettingAsync()) with { Webhook = string.Empty };
    }

    [HttpPost]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> UpdateTeamsIntegrationSetting([FromBody] TeamsAlertSetting request)
    {
        var result = await teamsAlertSettingService.UpdateSettingAsync(request);
        return result.GetResult();
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestTeamsIntegrationSetting()
    {
        var result = await teamsAlertSettingService.TestConnectionAsync();
        return result.GetResult();
    }
}