using CodeSecure.Application.Module.Setting;
using CodeSecure.Core.Extension;
using FluentResults;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace CodeSecure.Application.Services;

public record MailMessage
{
    public required IEnumerable<string> Receivers { get; set; }
    public required string Subject { get; set; }
    public required string Content { get; set; }
}

public interface ISmtpService
{
    Task<Result<bool>> SendAsync(MailMessage message);
    Task<Result<bool>> TestConnectionAsync(string receiver);
}

public class SmtpService(SmtpSetting setting) : ISmtpService
{
    public async Task<Result<bool>> SendAsync(MailMessage model)
    {
        if (!model.Receivers.Any())
        {
            return Result.Fail("There are not receiver");
        }

        try
        {
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(setting.Name, setting.UserName));
            foreach (var email in model.Receivers)
            {
                if (email.IsEmail())
                {
                    message.To.Add(new MailboxAddress(string.Empty, email));
                }
            }

            message.Subject = model.Subject;
            message.Body = new TextPart(TextFormat.Html) { Text = model.Content };
            using var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(setting.Server, setting.Port, setting.UseSsl);
            await mailClient.AuthenticateAsync(setting.UserName, setting.Password);
            await mailClient.SendAsync(message);
            await mailClient.DisconnectAsync(true);
            return true;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public Task<Result<bool>> TestConnectionAsync(string receiver)
    {
        return SendAsync(new MailMessage
        {
            Receivers = [receiver],
            Subject = "Test connection",
            Content = "This is a test content"
        });
    }
}