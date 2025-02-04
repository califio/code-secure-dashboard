using System.Net;
using System.Net.Mail;
using CodeSecure.Extension;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Manager.Integration.Mail;

public class MailSender(MailSetting setting, ILogger<IMailSender>? logger = null): IMailSender
{
    public async Task<NotificationResult> SendMailAsync(MailModel model)
    {
        if (!model.Receivers.Any())
        {
            return NotificationResult.Failed("There are not receiver");
        }
        var client = InitSmtpClient();
        if (client == null)
        {
            return NotificationResult.Failed("Can't init mail client");
        }
        try
        {
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.Subject = model.Subject;
            var sender = setting.UserName;
            message.From = new MailAddress(sender, "Code Secure");
            foreach (var email in model.Receivers)
            {
                if (email.IsEmail())
                {
                    message.To.Add(email); 
                }
                else
                {
                    logger?.LogWarning($"Invalid email: {email}");
                }
            }
            message.Body = TemplateEngine.Render(model.Template, model.Model);
            await client.SendMailAsync(message);
            return NotificationResult.Success;
        }
        catch (System.Exception e)
        {
            return NotificationResult.Failed(e.Message);
        }
    }

    public async Task<NotificationResult> SendTestMailAsync(string receiver)
    {
        logger?.LogInformation($"Send test mail to {receiver}");
        return await SendMailAsync(new MailModel
        {
            Receivers = [receiver],
            Subject = "Test Mail",
            Template = "Test mail success",
            Model = null
        });
    }

    public async Task SendInviteUser(IEnumerable<string> receivers, InviteUserModel model)
    {
        logger?.LogInformation($"send mail invite username {model.Username}");
        var template = GetTemplate("invite_user");
        var result = await SendMailAsync(new MailModel
        {
            Subject = "You have been invited to join Code Secure",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task SendResetPassword(IEnumerable<string> receivers, ResetPasswordModel model)
    {
        logger?.LogInformation($"send mail reset password for username {model.Username}");
        var template = GetTemplate("reset_password");
        var result = await SendMailAsync(new MailModel
        {
            Subject = "Password Reset Request",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task SendAddUserToProject(IEnumerable<string> receivers, AddUserToProjectModel model)
    {
        logger?.LogInformation($"send mail add user {model.Username} to project {model.ProjectName}");
        var template = GetTemplate("add_user_to_project");
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"You have been added to project {model.ProjectName}",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task SendRemoveProjectMember(IEnumerable<string> receivers, RemoveProjectMemberModel model)
    {
        logger?.LogInformation($"send mail remove {model.Username} from project {model.ProjectName}");
        var template = GetTemplate("remove_project_member");
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"You have been removed from project {model.ProjectName}",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }
    
    private static string GetTemplate(string name)
    {
        return File.ReadAllText(Path.Combine("Resources", "Templates", $"{name}.html"));
    }
    
    private SmtpClient? InitSmtpClient()
    {
        if (string.IsNullOrEmpty(setting.Server) || string.IsNullOrEmpty(setting.UserName))
        {
            return null;
        }
        return new SmtpClient
        {
            UseDefaultCredentials = false,
            Host = setting.Server,
            Port = setting.Port,
            Credentials = new NetworkCredential(setting.UserName, setting.Password),
            EnableSsl = setting.UseSsl
        };
    }
}