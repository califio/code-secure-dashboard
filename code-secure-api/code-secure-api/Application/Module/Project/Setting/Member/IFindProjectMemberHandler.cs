using System.Text.Json.Serialization;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Setting.Member;

public record ProjectMemberFilter : QueryFilter
{
    public string? Name { get; set; }
    public ProjectRole? Role { get; set; }
    
    [JsonIgnore]
    public Guid ProjectId { get; set; }
}

public interface IFindProjectMemberHandler : IHandler<ProjectMemberFilter, Page<ProjectMember>>;

public class FindProjectMemberHandler(AppDbContext context) : IFindProjectMemberHandler
{
    public async Task<Result<Page<ProjectMember>>> HandleAsync(ProjectMemberFilter request)
    {
        return await context.ProjectUsers
            .Include(record => record.User)
            .Where(record => record.ProjectId == request.ProjectId)
            .Where(record => request.Role == null || record.Role == request.Role)
            .Where(record => string.IsNullOrEmpty(request.Name) || record.User!.UserName!.Contains(request.Name!))
            .OrderBy(nameof(ProjectUsers.CreatedAt), request.Desc).Select(record => new ProjectMember
            {
                UserId = record.UserId,
                UserName = record.User!.UserName!,
                Avatar = record.User.Avatar,
                Role = record.Role,
                FullName = record.User.FullName,
                Email = record.User.Email,
                CreatedAt = record.CreatedAt
            }).PageAsync(request.Page, request.Size);
    }
}