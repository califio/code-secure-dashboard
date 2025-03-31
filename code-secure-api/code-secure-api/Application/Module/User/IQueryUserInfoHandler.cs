using CodeSecure.Core.EntityFramework;
using FluentResults;

namespace CodeSecure.Application.Module.User;

public interface IQueryUserInfoHandler : IHandler<UserFilter, Page<UserInfo>>;
public class QueryUserInfoHandler(AppDbContext context): IQueryUserInfoHandler
{
    public async Task<Result<Page<UserInfo>>> HandleAsync(UserFilter request)
    {
        var query = from user in context.Users
            join userRoles in context.UserRoles on user.Id equals userRoles.UserId
            join role in context.Roles on userRoles.RoleId equals role.Id
            select new
            {
                user, role
            };
        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(record => record.user.UserName!.Contains(request.Name));

        if (request.RoleId != null) query = query.Where(record => record.role.Id == request.RoleId);

        if (request.Status != null) query = query.Where(record => record.user.Status == request.Status);

        return await query.Select(record => new UserInfo
        {
            Id = record.user.Id,
            UserName = record.user.UserName!,
            FullName = record.user.FullName,
            Avatar = record.user.Avatar,
            Status = record.user.Status,
            Email = record.user.Email!,
            CreatedAt = record.user.CreatedAt,
            UpdatedAt = record.user.UpdatedAt,
            Role = record.role.Name!,
            Enable2Fa = record.user.TwoFactorEnabled,
            Verified = record.user.EmailConfirmed,
            Lockout = record.user.LockoutEnd
        }).OrderBy(request.SortBy.ToString(), request.Desc).PageAsync(request.Page, request.Size);
    }
}