using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

[Route("api/integration/jira-webhook")]
[ApiExplorerSettings(GroupName = "Integration")]
public class JiraWebhookIntegrationController(IJiraWebhookSettingService jiraWebhookSettingService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<JiraWebhookSetting> GetJiraWebhookIntegrationSetting()
    {
        var setting = await jiraWebhookSettingService.GetSettingAsync();
        return setting with { Token = string.Empty };
    }

    [HttpPost]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> UpdateJiraWebhookIntegrationSetting([FromBody] JiraWebhookSetting request)
    {
        var result = await jiraWebhookSettingService.UpdateSettingAsync(request);
        return result.GetResult();
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestJiraWebhookIntegrationSetting()
    {
        var result = await jiraWebhookSettingService.TestConnectionAsync();
        return result.GetResult();
    }
}