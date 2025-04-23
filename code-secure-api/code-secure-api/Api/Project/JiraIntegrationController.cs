using CodeSecure.Application.Module.Integration.Jira.Client;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration.Jira;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class JiraIntegrationController(
    IProjectAuthorize projectAuthorize,
    IJiraProjectIntegrationSetting jiraProjectIntegrationSetting
) : BaseController
{
    [HttpGet]
    [Route("{projectId:guid}/integration/jira")]
    public async Task<JiraProjectSetting> GetJiraIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await jiraProjectIntegrationSetting.GetSettingAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId:guid}/integration/jira")]
    public async Task<bool> UpdateJiraIntegrationProject(Guid projectId, [FromBody] JiraProjectSetting request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await jiraProjectIntegrationSetting.UpdateSettingAsync(projectId, request);
        return result.GetResult();
    }

    [HttpGet]
    [Route("{projectId:guid}/integration/jira/projects")]
    public async Task<List<JiraProject>> ListJiraProjects(Guid projectId, bool reload)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await jiraProjectIntegrationSetting.ListJiraProjectsAsync(reload);
        return result.GetResult();
    }
}