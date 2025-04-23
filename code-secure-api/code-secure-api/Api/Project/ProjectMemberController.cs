using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Setting.Member;
using CodeSecure.Authentication;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class ProjectMemberController(
    IProjectAuthorize projectAuthorize,
    IFindProjectMemberHandler findProjectMemberHandler,
    ICreateProjectMemberHandler createProjectMemberHandler,
    IUpdateProjectMemberHandler updateProjectMemberHandler,
    IDeleteProjectMemberHandler deleteProjectMemberHandler
) : BaseController
{
    [HttpPost]
    [Route("{projectId}/member/filter")]
    public async Task<Page<ProjectMember>> GetProjectUsers(Guid projectId, ProjectMemberFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        filter.ProjectId = projectId;
        var result = await findProjectMemberHandler.HandleAsync(filter);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId}/member")]
    public async Task<ProjectMember> AddMember(Guid projectId, CreateProjectMemberRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        request.CurrentUserId = CurrentUser().Id;
        var result = await createProjectMemberHandler.HandleAsync(request);
        return result.GetResult();
    }

    [HttpPatch]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<ProjectMember> UpdateProjectMember(Guid projectId, Guid userId,
        UpdateProjectMemberRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        request.UserId = userId;
        var result = await updateProjectMemberHandler.HandleAsync(request);
        return result.GetResult();
    }

    [HttpDelete]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<bool> DeleteProjectMember(Guid projectId, Guid userId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await deleteProjectMemberHandler.HandleAsync(new DeleteProjectMemberRequest
        {
            ProjectId = projectId,
            UserId = userId
        });
        return result.GetResult();
    }
}