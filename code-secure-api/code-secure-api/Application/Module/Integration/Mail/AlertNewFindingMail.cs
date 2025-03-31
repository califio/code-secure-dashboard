using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertNewFindingMail(ISmtpService smtpService, IRazorRender render) : IAlertNewFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertStatusFindingModel model)
    {
        var content = await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertNewFinding.cshtml"), model);
        return await smtpService.SendAsync(new MailMessage
        {
            Subject =
                $"Security Alert: Found new finding on \"{model.Project.Name}\" project by {model.Scanner.Name}",
            Receivers = receivers,
            Content = content,
        });
    }
}