using CodeSecure.Application.Module.Auth.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Auth.Command;

public class GetUserProfileCommand(AppDbContext context)
{
    public async Task<Result<UserProfile>> ExecuteAsync(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null) return Result.Fail("User not found");
        return new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName!,
            FullName = user.FullName,
            Email = user.Email,
            Avatar = user.Avatar
        };
    }
}