using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CodeSecure.Application.Module.Mail;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Setting.Member;

public record CreateProjectMemberRequest
{
    [Required] public required Guid UserId { get; init; }

    [Required] public required ProjectRole Role { get; init; }
    [JsonIgnore] public Guid ProjectId { get; set; }
    [JsonIgnore] public Guid CurrentUserId { get; set; }
}

public interface ICreateProjectMemberHandler : IHandler<CreateProjectMemberRequest, ProjectMember>;

public class CreateProjectMemberHandler(AppDbContext context, IMailAddUserToProject mailAddUserToProject)
    : ICreateProjectMemberHandler
{
    public async Task<Result<ProjectMember>> HandleAsync(CreateProjectMemberRequest request)
    {
        var project = await context.Projects.FirstOrDefaultAsync(project => project.Id == request.ProjectId);
        if (project == null) return Result.Fail("Project not found");
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId);
        if (user == null) return Result.Fail("User not found");
        if (context.ProjectUsers.Any(record =>
                record.ProjectId == request.ProjectId && record.UserId == request.UserId))
        {
            return Result.Fail("User already exists");
        }

        var projectUser = new ProjectUsers
        {
            UserId = user.Id,
            ProjectId = request.ProjectId,
            AddedById = request.CurrentUserId,
            Role = request.Role,
            ReceiveNotification = true,
            CreatedAt = DateTime.UtcNow,
        };
        context.ProjectUsers.Add(projectUser);
        await context.SaveChangesAsync();
        _ = mailAddUserToProject.SendAsync([user.Email!], new MailAddUserToProjectModel
        {
            Role = request.Role,
            Username = user.UserName!,
            Project = project,
        });
        return new ProjectMember
        {
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            FullName = user.FullName,
            Email = user.Email,
            Avatar = user.Avatar,
            Role = projectUser.Role,
            CreatedAt = projectUser.CreatedAt
        };
    }
}