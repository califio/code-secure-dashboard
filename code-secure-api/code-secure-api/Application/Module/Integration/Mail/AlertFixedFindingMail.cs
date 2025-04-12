using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertFixedFindingMail(IRazorRender render, ISmtpService smtpService): IAlertFixedFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertStatusFindingModel model)
    {
        try
        {
            var content = await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertFixedFinding.cshtml"), model);
            return await smtpService.SendAsync(new MailMessage
            {
                Subject = $"Notification: Some findings have been fixed on \"{model.Project.Name}\" project",
                Receivers = receivers,
                Content = content,
            });
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}