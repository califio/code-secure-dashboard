using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User;

public interface IFindUserByIdHandler : IHandler<Guid, UserInfo>;

public class FindUserByIdHandler(AppDbContext context) : IFindUserByIdHandler
{
    public async Task<Result<UserInfo>> HandleAsync(Guid request)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        var role = await context.Roles.FirstAsync(role => context.UserRoles.Any(record =>
            record.RoleId == role.Id && record.UserId == user.Id)
        );
        return new UserInfo
        {
            Status = user.Status,
            Email = user.Email!,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Role = role.Name!,
            Enable2Fa = user.TwoFactorEnabled,
            Id = user.Id,
            UserName = user.FullName,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Verified = user.EmailConfirmed,
            Lockout = user.LockoutEnd
        };
    }
}