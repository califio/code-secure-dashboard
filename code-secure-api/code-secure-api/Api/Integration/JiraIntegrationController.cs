using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Jira.Client;
using CodeSecure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

[Route("api/integration/jira")]
[ApiExplorerSettings(GroupName = "Integration")]
public class JiraIntegrationController(IJiraSettingService jiraSettingService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<JiraSetting> GetJiraIntegrationSetting()
    {
        var setting = await jiraSettingService.GetSettingAsync();
        return setting with { Password = string.Empty };
    }

    [HttpPost]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task<bool> UpdateJiraIntegrationSetting([FromBody] JiraSetting request)
    {
        return jiraSettingService.UpdateSettingAsync(request);
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task<bool> TestJiraIntegrationSetting()
    {
        return jiraSettingService.TestConnectionAsync();
    }

    [HttpPost]
    [Route("projects")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<List<JiraProject>> GetJiraProjects([FromBody] JiraSetting? setting = null, bool reload = false)
    {
        setting ??= await jiraSettingService.GetSettingAsync();
        var jiraInstance = new JiraClient(new JiraConnection
        {
            Url = setting.WebUrl,
            Password = setting.Password,
            Username = setting.UserName
        });
        return await jiraInstance.GetProjectsSummaryAsync(reload);
    }

    [HttpPost]
    [Route("issue-types")]
    public async Task<List<string>> GetJiraIssueTypes(string? projectKey)
    {
        if (string.IsNullOrEmpty(projectKey))
        {
            return [];
        }

        var jiraSetting = await jiraSettingService.GetSettingAsync();
        return await new JiraClient(new JiraConnection
        {
            Url = jiraSetting.WebUrl,
            Password = jiraSetting.Password,
            Username = jiraSetting.UserName
        }).GetIssueTypesForProjectAsync(projectKey);
    }
}