using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Authentication;
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
    public Task<bool> UpdateTeamsIntegrationSetting([FromBody] TeamsAlertSetting request)
    {
        return teamsAlertSettingService.UpdateSettingAsync(request);
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task<bool> TestTeamsIntegrationSetting()
    {
        return teamsAlertSettingService.TestConnectionAsync();
    }
}