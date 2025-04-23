using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Setting.Threshold;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class ProjectThresholdController(
    IProjectAuthorize projectAuthorize,
    IGetProjectThresholdHandler getProjectThresholdHandler,
    IUpdateProjectThresholdHandler updateProjectThresholdHandler
) : BaseController
{
    [HttpGet]
    [Route("{projectId}/threshold")]
    public async Task<ThresholdProject> GetThresholdProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await getProjectThresholdHandler.HandleAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId}/threshold")]
    public async Task<bool> UpdateThresholdProject(Guid projectId, UpdateProjectThresholdRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        var result = await updateProjectThresholdHandler.HandleAsync(request);
        return result.GetResult();
    }
}