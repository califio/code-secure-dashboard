using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Mail;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth;

public class ForgotPasswordRequest
{
    [Required] public required string Username { get; set; }
}

public interface IForgotPasswordHandler : IHandler<ForgotPasswordRequest, bool>;

public class ForgotPasswordHandler(JwtUserManager userManager, IMailResetPassword mailResetPassword)
    : IForgotPasswordHandler
{
    public async Task<Result<bool>> HandleAsync(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username) ??
                   await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        _ = mailResetPassword.SendAsync(user.Email!, new MailResetPasswordModel
        {
            Username = user.UserName!,
            Token = await userManager.GeneratePasswordResetTokenAsync(user)
        });
        return Result.Ok();
    }
}