using CodeSecure.Application.Module.Mail;
using CodeSecure.Application.Module.Project.Command;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Project;

public interface IProjectMemberService
{
    Task<ProjectMember> CreateMemberAsync(Guid projectId, CreateProjectMemberRequest request);
    Task<ProjectMember> UpdateMemberAsync(Guid projectId, UpdateProjectMemberRequest request);
    Task<bool> DeleteMemberAsync(Guid projectId, Guid userId);
    Task<Page<ProjectMember>> GetMemberByFilterAsync(Guid projectId, ProjectMemberFilter filter);
}

public class ProjectMemberService(
    IHttpContextAccessor accessor,
    AppDbContext context,
    IMailAddUserToProject mailAddUserToProject,
    IMailRemoveUserFromProject mailRemoveUserFromProject
) : BaseService(accessor), IProjectMemberService
{
    public async Task<ProjectMember> CreateMemberAsync(Guid projectId, CreateProjectMemberRequest request)
    {
        return (await new CreateProjectMemberCommand(context, CurrentUser, mailAddUserToProject)
            .ExecuteAsync(projectId, request)).GetResult();
    }

    public async Task<ProjectMember> UpdateMemberAsync(Guid projectId, UpdateProjectMemberRequest request)
    {
        return (await new UpdateProjectMemberCommand(context)
            .ExecuteAsync(projectId, request)).GetResult();
    }

    public async Task<bool> DeleteMemberAsync(Guid projectId, Guid userId)
    {
        return (await new DeleteProjectMemberCommand(context, mailRemoveUserFromProject)
            .ExecuteAsync(new DeleteProjectMemberRequest
            {
                ProjectId = projectId,
                UserId = userId
            })).GetResult();
    }

    public async Task<Page<ProjectMember>> GetMemberByFilterAsync(Guid projectId, ProjectMemberFilter filter)
    {
        return (await new GetProjectMemberByFilterCommand(context)
            .ExecuteAsync(projectId, filter)).GetResult();
    }
}