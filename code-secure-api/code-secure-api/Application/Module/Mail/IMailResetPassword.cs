using CodeSecure.Application.Services;
using CodeSecure.Core.Extension;
using FluentResults;

namespace CodeSecure.Application.Module.Mail;

public record MailResetPasswordModel
{
    public required string Username { get; set; }

    public required string Token { get; set; }

    public string ResetPasswordLink()
    {
        return $"{Configuration.FrontendUrl}/#/auth/reset-password?token={Token.UrlEncode()}&username={Username}";
    }
}

public interface IMailResetPassword
{
    Task<Result<bool>> SendAsync(string receiver, MailResetPasswordModel model);
}

public class MailResetPassword(ISmtpService smtpService, IRazorRender render) : IMailResetPassword
{
    public async Task<Result<bool>> SendAsync(string receiver, MailResetPasswordModel model)
    {
        var content =
            await render.RenderAsync(Path.Combine("Resources", "Templates", "MailResetPassword.cshtml"), model);
        return await smtpService.SendAsync(new MailMessage
        {
            Subject = "Password Reset Request",
            Receivers = [receiver],
            Content = content,
        });
    }
}