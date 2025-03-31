using CodeSecure.Application.Services;
using CodeSecure.Core.Extension;
using FluentResults;

namespace CodeSecure.Application.Module.Mail;

public record MailInviteUserModel
{
    public required string Username { get; init; }
    public required string Token { get; init; }
    public bool IsRegister { get; init; }

    public string ConfirmInviteLink()
    {
        return $"{Configuration.FrontendUrl}/#/auth/confirm-email?token={Token.UrlEncode()}&username={Username}";
    }
}
public interface IMailInviteUser
{
    public Task<Result<bool>> SendAsync(string receiver, MailInviteUserModel model);
}

public class MailInviteUser(ISmtpService smtpService, IRazorRender render) : IMailInviteUser
{
    public async Task<Result<bool>> SendAsync(string receiver, MailInviteUserModel model)
    {
        var content =
            await render.RenderAsync(Path.Combine("Resources", "Templates", "MailInviteUser.cshtml"), model);
        return await smtpService.SendAsync(new MailMessage
        {
            Subject = $"You have been invited to join {Configuration.AppName}",
            Receivers = [receiver],
            Content = content,
        });
    }
}