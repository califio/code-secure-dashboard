using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Jira.Client;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
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
    public async Task<bool> UpdateJiraIntegrationSetting([FromBody] JiraSetting request)
    {
        var result = await jiraSettingService.UpdateSettingAsync(request);
        return result.GetResult();
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestJiraIntegrationSetting()
    {
        var result = await jiraSettingService.TestConnectionAsync();
        return result.GetResult();
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
    //[Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<List<string>> GetJiraIssueTypes([Required] string projectKey)
    {
        var jiraSetting = await jiraSettingService.GetSettingAsync();
        return await new JiraClient(new JiraConnection
        {
            Url = jiraSetting.WebUrl,
            Password = jiraSetting.Password,
            Username = jiraSetting.UserName
        }).GetIssueTypesForProjectAsync(projectKey);
    }
}