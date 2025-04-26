using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;


public class GetProjectMemberByFilterCommand(AppDbContext context)
{
    public async Task<Result<Page<ProjectMember>>> ExecuteAsync(Guid projectId, ProjectMemberFilter filter)
    {
        return await context.ProjectUsers
            .Include(record => record.User)
            .Where(record => record.ProjectId == projectId)
            .Where(record => filter.Role == null || record.Role == filter.Role)
            .Where(record => string.IsNullOrEmpty(filter.Name) || record.User!.UserName!.Contains(filter.Name!))
            .OrderBy(nameof(ProjectUsers.CreatedAt), filter.Desc).Select(record => new ProjectMember
            {
                UserId = record.UserId,
                UserName = record.User!.UserName!,
                Avatar = record.User.Avatar,
                Role = record.Role,
                FullName = record.User.FullName,
                Email = record.User.Email,
                CreatedAt = record.CreatedAt
            }).PageAsync(filter.Page, filter.Size);
    }
}