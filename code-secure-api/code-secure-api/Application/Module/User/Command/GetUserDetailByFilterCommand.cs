using CodeSecure.Application.Module.User.Model;
using CodeSecure.Core.EntityFramework;
using FluentResults;

namespace CodeSecure.Application.Module.User.Command;
public class GetUserDetailByFilterCommand(AppDbContext context)
{
    public async Task<Result<Page<UserDetail>>> ExecuteAsync(UserFilter filter)
    {
        var query = from user in context.Users
            join userRoles in context.UserRoles on user.Id equals userRoles.UserId
            join role in context.Roles on userRoles.RoleId equals role.Id
            select new
            {
                user, role
            };
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(record => record.user.UserName!.Contains(filter.Name));

        if (filter.RoleId != null) query = query.Where(record => record.role.Id == filter.RoleId);

        if (filter.Status != null) query = query.Where(record => record.user.Status == filter.Status);

        return await query.Select(record => new UserDetail
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
        }).OrderBy(filter.SortBy.ToString(), filter.Desc).PageAsync(filter.Page, filter.Size);
    }
}