using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User;

public class UpdateUserRequest
{
    [Required] public Guid UserId { get; set; }
    [StringLength(64)] public string? FullName { get; set; }

    [StringLength(64)] [EmailAddress] public string? Email { get; set; }
    public bool? Verified { get; set; }
    public UserStatus? Status { get; set; }
    public string? Role { get; set; }
}

public interface IUpdateUserHandler : IHandler<UpdateUserRequest, Users>;

public class UpdateUserHandler(AppDbContext context, JwtUserManager userManager) : IUpdateUserHandler
{
    public async Task<Result<Users>> HandleAsync(UpdateUserRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId);
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