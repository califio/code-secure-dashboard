using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Mail;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.User;

public record CreateUserRequest
{
    [EmailAddress] public required string Email { get; set; }
    public bool Verified { get; set; }
    [Required] public required string Role { get; set; }
}

public interface ICreateUserHandler : IHandler<CreateUserRequest, Users>;

public class CreateUserHandler(JwtUserManager userManager, IMailInviteUser mailInviteUser) : ICreateUserHandler
{
    public async Task<Result<Users>> HandleAsync(CreateUserRequest request)
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
        SendConfirmEmail(user);
        return user;
    }

    private void SendConfirmEmail(Users user)
    {
        mailInviteUser.SendAsync(user.Email!, new MailInviteUserModel
        {
            Username = user.UserName!,
            Token = userManager.GenerateEmailConfirmationTokenAsync(user).Result,
            IsRegister = true
        });
    }
}