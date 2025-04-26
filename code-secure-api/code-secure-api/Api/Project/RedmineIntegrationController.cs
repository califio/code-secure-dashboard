using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration.Redmine;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class RedmineIntegrationController(
    IProjectAuthorize projectAuthorize,
    IRedmineProjectIntegrationSetting redmineProjectIntegrationSetting
) : BaseController
{
    [HttpGet]
    [Route("{projectId:guid}/integration/redmine")]
    public async Task<RedmineProjectSetting> GetRedmineIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        var result = await redmineProjectIntegrationSetting.GetSettingAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId:guid}/integration/redmine")]
    public async Task<bool> UpdateRedmineIntegrationProject(Guid projectId, [FromBody] RedmineProjectSetting request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        var result = await redmineProjectIntegrationSetting.UpdateSettingAsync(projectId, request);
        return result.GetResult();
    }

    [HttpGet]
    [Route("{projectId:guid}/integration/redmine/metadata")]
    public async Task<RedmineMetadata> GetRedmineMetadata(Guid projectId, bool reload)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        var result = await redmineProjectIntegrationSetting.GetRedmineMetadataAsync(reload);
        return result.GetResult();
    }
}