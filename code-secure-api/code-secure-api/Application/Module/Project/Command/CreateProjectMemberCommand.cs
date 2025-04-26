using CodeSecure.Application.Module.Mail;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class CreateProjectMemberCommand(AppDbContext context, JwtUserClaims currentUser, IMailAddUserToProject mailAddUserToProject)
{
    public async Task<Result<ProjectMember>> ExecuteAsync(Guid projectId, CreateProjectMemberRequest request)
    {
        var project = await context.Projects.FirstOrDefaultAsync(project => project.Id == projectId);
        if (project == null) return Result.Fail("Project not found");
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId);
        if (user == null) return Result.Fail("User not found");
        if (context.ProjectUsers.Any(record =>
                record.ProjectId == projectId && record.UserId == request.UserId))
        {
            return Result.Fail("User already exists");
        }

        var projectUser = new ProjectUsers
        {
            UserId = user.Id,
            ProjectId = projectId,
            AddedById = currentUser.Id,
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