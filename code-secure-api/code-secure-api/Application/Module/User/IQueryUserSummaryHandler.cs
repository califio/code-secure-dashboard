using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.User;

public interface IQueryUserSummaryHandler : IHandler<UserFilter, Page<UserSummary>>;

public class QueryUserSummaryHandler(AppDbContext context) : IQueryUserSummaryHandler
{
    public async Task<Result<Page<UserSummary>>> HandleAsync(UserFilter request)
    {
        var query = context.Users.Where(user => user.Status != UserStatus.Disabled && user.IsDefault == false);
        if (!string.IsNullOrEmpty(request.Name)) query = query.Where(user => user.UserName!.Contains(request.Name));

        if (request.RoleId != null)
            query = query.Where(user =>
                context.UserRoles.Any(userRole => userRole.UserId == user.Id && userRole.RoleId == request.RoleId));

        return await query.OrderBy(nameof(Users.CreatedAt), request.Desc).Select(user => new UserSummary
        {
            Id = user.Id,
            UserName = user.UserName!,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Email = user.Email
        }).PageAsync(request.Page, request.Size);
    }
}