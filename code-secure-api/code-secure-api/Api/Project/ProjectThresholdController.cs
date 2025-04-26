using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class ProjectThresholdController(
    IProjectAuthorize projectAuthorize,
    IProjectSettingService projectSettingService
) : BaseController
{
    [HttpGet]
    [Route("{projectId}/threshold")]
    public async Task<ThresholdProject> GetThresholdProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectSettingService.GetProjectThresholdAsync(projectId);
    }

    [HttpPost]
    [Route("{projectId}/threshold")]
    public async Task<bool> UpdateThresholdProject(Guid projectId, UpdateProjectThresholdRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        return await projectSettingService.UpdateProjectThresholdAsync(projectId, request);
    }
}