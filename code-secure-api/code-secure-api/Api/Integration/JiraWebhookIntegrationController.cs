using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Authentication;
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
    public Task<bool> UpdateJiraWebhookIntegrationSetting([FromBody] JiraWebhookSetting request)
    {
        return jiraWebhookSettingService.UpdateSettingAsync(request);
    }
}