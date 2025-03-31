using CodeSecure.Application.Module.Mail;
using CodeSecure.Authentication.Jwt;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User;

public interface ISendConfirmEmailHandler : IHandler<Guid, bool>;

public class SendConfirmEmailHandler(
    AppDbContext context,
    JwtUserManager userManager,
    IMailInviteUser mailInviteUser
) : ISendConfirmEmailHandler
{
    public async Task<Result<bool>> HandleAsync(Guid request)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Fail("This account was confirmed");
        }

        _ = mailInviteUser.SendAsync(user.Email!, new MailInviteUserModel
        {
            Username = user.UserName!,
            Token = userManager.GenerateEmailConfirmationTokenAsync(user).Result
        });
        return true;
    }
}