using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class ProjectMemberController(
    IProjectAuthorize projectAuthorize,
    IProjectMemberService projectMemberService
) : BaseController
{
    [HttpPost]
    [Route("{projectId}/member/filter")]
    public async Task<Page<ProjectMember>> GetProjectUsers(Guid projectId, ProjectMemberFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectMemberService.GetMemberByFilterAsync(projectId, filter);
    }

    [HttpPost]
    [Route("{projectId}/member")]
    public async Task<ProjectMember> AddMember(Guid projectId, CreateProjectMemberRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        return await projectMemberService.CreateMemberAsync(projectId, request);
    }

    [HttpPatch]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<ProjectMember> UpdateProjectMember(Guid projectId, Guid userId,
        UpdateProjectMemberRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        request.UserId = userId;
        return await projectMemberService.UpdateMemberAsync(projectId, request);
    }

    [HttpDelete]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<bool> DeleteProjectMember(Guid projectId, Guid userId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        return await projectMemberService.DeleteMemberAsync(projectId, userId);
    }
}