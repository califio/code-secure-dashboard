using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertVulnerableProjectPackageMail(ISmtpService smtpService, IRazorRender render)
    : IAlertVulnerableProjectPackage
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertVulnerableProjectPackageModel model)
    {
        var content =
            await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertVulnerableProjectPackage.cshtml"), model);
        return await smtpService.SendAsync(new MailMessage
        {
            Subject = $"Security Alert: Vulnerability found in dependencies of \"{model.Project.Name}\"",
            Receivers = receivers,
            Content = content,
        });
    }
}