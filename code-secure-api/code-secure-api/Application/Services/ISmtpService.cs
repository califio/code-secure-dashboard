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
            message.From.Add(setting.UserName.IsEmail()
                ? new MailboxAddress(setting.Name, setting.UserName)
                : new MailboxAddress(setting.Name, setting.Name));

            foreach (var email in model.Receivers)
            {
                if (MailboxAddress.TryParse(email, out var address))
                {
                    message.To.Add(address);
                }
            }

            message.Subject = model.Subject;
            message.Body = new TextPart(TextFormat.Html) { Text = model.Content };
            using var mailClient = new SmtpClient();
            if (setting.IgnoreSsl)
            {
                mailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
            }

            if (setting.UseSsl)
            {
                await mailClient.ConnectAsync(setting.Server, setting.Port, setting.UseSsl);
            }
            else
            {
                await mailClient.ConnectAsync(setting.Server, setting.Port);
            }

            await mailClient.AuthenticateAsync(setting.UserName, setting.Password);
            await mailClient.SendAsync(message);
            await mailClient.DisconnectAsync(true);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
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