using CodeSecure.Application.Module.Mail;
using CodeSecure.Application.Module.User.Model;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.User.Command;

public class CreateUserCommand(JwtUserManager userManager, IMailInviteUser mailInviteUser)
{
    public async Task<Result<Users>> ExecuteAsync(CreateUserRequest request)
    {
        var username = request.Email.Split('@')[0];
        var user = new Users
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = request.Email,
            EmailConfirmed = request.Verified,
            TwoFactorEnabled = false,
            FullName = username,
            Status = UserStatus.Active,
            Avatar = null,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, PasswordGenerator.GeneratePassword(32));
        if (!result.Succeeded) return Result.Fail(result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, request.Role);
        _ = mailInviteUser.SendAsync(user.Email!, new MailInviteUserModel
        {
            Username = user.UserName!,
            Token = userManager.GenerateEmailConfirmationTokenAsync(user).Result,
            IsRegister = true
        });
        return user;
    }
}