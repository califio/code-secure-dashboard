using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

[Route("api/integration/redmine")]
[ApiExplorerSettings(GroupName = "Integration")]
public class RedmineIntegrationController(IRedmineSettingService redmineSettingService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<RedmineSetting> GetRedmineIntegrationSetting()
    {
        var setting = await redmineSettingService.GetSettingAsync();
        return setting with { Token = string.Empty };
    }

    [HttpPost]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task<bool> UpdateRedmineIntegrationSetting([FromBody] RedmineSetting request)
    {
        return redmineSettingService.UpdateSettingAsync(request);
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task<bool> TestRedmineIntegrationSetting()
    {
        return redmineSettingService.TestConnectionAsync();
    }

    [HttpPost]
    [Route("metadata")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<RedmineMetadata> GetRedmineMetadataIntegration([FromBody] RedmineSetting? setting = null,
        bool reload = false)
    {
        setting ??= await redmineSettingService.GetSettingAsync();
        if (string.IsNullOrEmpty(setting.Url) || string.IsNullOrEmpty(setting.Token))
        {
            throw new BadRequestException("url or token is missing");
        }

        var redmineClient = new RedmineClient(setting.Url, setting.Token);
        return await redmineClient.GetMetadataAsync(reload);
    }
}