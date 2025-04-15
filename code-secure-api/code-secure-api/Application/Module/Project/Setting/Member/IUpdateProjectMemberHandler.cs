using System.Text.Json.Serialization;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Setting.Member;

public record UpdateProjectMemberRequest
{
    public required ProjectRole Role { get; set; }
    [JsonIgnore] public Guid ProjectId { get; set; }
    [JsonIgnore] public Guid UserId { get; set; }
}

public interface IUpdateProjectMemberHandler : IHandler<UpdateProjectMemberRequest, ProjectMember>;

public class UpdateProjectMemberHandler(AppDbContext context) : IUpdateProjectMemberHandler
{
    public async Task<Result<ProjectMember>> HandleAsync(UpdateProjectMemberRequest request)
    {
        var projectUser = await context.ProjectUsers
            .Include(record => record.User)
            .FirstOrDefaultAsync(record => record.ProjectId == request.ProjectId && record.UserId == request.UserId);
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