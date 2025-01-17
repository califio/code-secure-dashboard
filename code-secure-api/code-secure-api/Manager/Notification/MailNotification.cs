using System.Net;
using System.Net.Mail;
using CodeSecure.Database.Metadata;
using CodeSecure.Extension;
using CodeSecure.Manager.Notification.Model;

namespace CodeSecure.Manager.Notification;

public class MailNotification(MailSetting setting, ILogger<INotification>? logger = null): INotification
{
    public async Task<NotificationResult> PushTestNotification(string email)
    {
        return await SendMailAsync(new MailModel
        {
            Receivers = [email],
            Subject = "Test Mail",
            Template = "Test mail success",
            Model = null
        });
    }

    public async Task PushScanInfo(IEnumerable<string> receivers, ScanInfoModel model)
    {
        logger?.LogInformation($"send mail scan result {model.ScanName} on {model.ProjectName}");
        var template = GetTemplate("scan_info");
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"Scan on \"{model.ProjectName}\" by {model.ScannerName} completed",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task PushNewFindingInfo(IEnumerable<string> receivers, NewFindingInfoModel model)
    {
        if (model.Findings.Count == 0)
        {
            return;
        }
        logger?.LogInformation($"send mail new finding on {model.ProjectName} by {model.ScannerName} scanner");
        var template = GetTemplate("new_finding_info");
        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"Security Alert: Found new finding on \"{model.ProjectName}\" project by {model.ScannerName} - {model.ScannerType}",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task PushFixedFindingInfo(IEnumerable<string> receivers, FixedFindingInfoModel model)
    {
        if (model.Findings.Count == 0)
        {
            return;
        }
        logger?.LogInformation($"send mail notify fixed finding on {model.ProjectName}");
        var template = GetTemplate("fixed_finding_info");
        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"Notification: Some findings have been fixed on \"{model.ProjectName}\" project",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task PushNeedsTriageFindingInfo(IEnumerable<string> receivers, NeedsTriageFindingInfoModel model)
    {
        logger?.LogInformation($"send mail NeedsTriageFindingInfo on {model.ProjectName}");
        var template = GetTemplate("needs_triage_finding_info");
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"Reminder: Please verify unconfirmed finding on \"{model.ProjectName}\" project",
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task PushDependencyReport(IEnumerable<string> receivers, DependencyReportModel model, string? subject = null)
    {
        logger?.LogInformation($"send mail dependency report repo {model.RepoName}");
        var template = GetTemplate("dependency_report");
        subject ??= $"Security Alert: Vulnerability found in dependencies of \"{model.RepoName}\"";
        var result = await SendMailAsync(new MailModel
        {
            Subject = subject,
            Receivers = receivers,
            Template = template,
            Model = model,
        });
        if (!result.Succeeded)
        {
            logger?.LogError(result.Error);
        }
    }

    public async Task PushAlertProjectWithoutMember(IEnumerable<string> receivers, AlertProjectWithoutMemberModel model)
    {
        logger?.LogInformation($"send mail alert project without member: {model.ProjectName}");
        var template = GetTemplate("alert_project_without_member");
        var result = await SendMailAsync(new MailModel
        {
            Subject = $"Action Required: Add at least one member to {model.ProjectName} to receive notifications",
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