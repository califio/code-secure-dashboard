using CodeSecure.Application.Module.User.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User.Command;

public class UpdateUserCommand(AppDbContext context, JwtUserManager userManager)
{
    public async Task<Result<Users>> ExecuteAsync(Guid userId, UpdateUserRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            user.Email = request.Email;
            user.UserName = request.Email.Split('@')[0];
        }

        if (!string.IsNullOrEmpty(request.FullName) && request.FullName != user.FullName)
        {
            user.FullName = request.FullName;
        }

        if (request.Verified != null)
        {
            user.EmailConfirmed = (bool)request.Verified;
        }

        if (request.Status != null && request.Status != user.Status) user.Status = (UserStatus)request.Status;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Fail(result.Errors.First().Description);
        }

        if (!string.IsNullOrEmpty(request.Role))
        {
            if (await userManager.IsInRoleAsync(user, request.Role!) == false)
            {
                // delete all current role of user
                await context.UserRoles.Where(record => record.UserId == user.Id).ExecuteDeleteAsync();
                result = await userManager.AddToRoleAsync(user, request.Role!);
                if (!result.Succeeded)
                {
                    return Result.Fail(result.Errors.First().Description);
                }
            }
        }

        return user;
    }
}