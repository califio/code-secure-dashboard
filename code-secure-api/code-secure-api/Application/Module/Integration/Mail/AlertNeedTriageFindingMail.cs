using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertNeedTriageFindingMail(ISmtpService smtpService, IRazorRender render): IAlertNeedTriageFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertNeedTriageFindingModel model)
    {
        var content = await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertNeedTriageFinding.cshtml"), model);
        return await smtpService.SendAsync(new MailMessage
        {
            Subject = $"Reminder: Please verify unconfirmed finding on \"{model.Project.Name}\" project",
            Receivers = receivers,
            Content = content,
        });
    }
}