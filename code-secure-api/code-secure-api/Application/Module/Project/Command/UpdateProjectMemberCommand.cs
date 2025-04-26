using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class UpdateProjectMemberCommand(AppDbContext context) 
{
    public async Task<Result<ProjectMember>> ExecuteAsync(Guid projectId, UpdateProjectMemberRequest request)
    {
        var projectUser = await context.ProjectUsers
            .Include(record => record.User)
            .FirstOrDefaultAsync(record => record.ProjectId == projectId && record.UserId == request.UserId);
        if (projectUser == null) return Result.Fail("Project user not found");
        projectUser.Role = request.Role;
        context.ProjectUsers.Update(projectUser);
        await context.SaveChangesAsync();
        return new ProjectMember
        {
            UserId = projectUser.UserId,
            UserName = projectUser.User!.UserName ?? string.Empty,
            FullName = projectUser.User!.FullName,
            Email = projectUser.User!.Email,
            Avatar = projectUser.User!.Avatar,
            Role = projectUser.Role,
            CreatedAt = projectUser.CreatedAt
        };
    }
}