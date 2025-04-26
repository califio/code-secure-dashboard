using CodeSecure.Application.Module.User.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User.Command;

public class GetUserByIdCommand(AppDbContext context)
{
    public async Task<Result<UserDetail>> ExecuteAsync(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        var role = await context.Roles.FirstAsync(role => context.UserRoles.Any(record =>
            record.RoleId == role.Id && record.UserId == user.Id)
        );
        return new UserDetail
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