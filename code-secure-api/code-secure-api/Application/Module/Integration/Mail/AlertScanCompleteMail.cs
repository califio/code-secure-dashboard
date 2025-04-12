using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertScanCompleteMail(ISmtpService smtpService, IRazorRender render): IAlertScanComplete
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertScanCompleteModel model)
    {
        try
        {
            var content = await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertScanComplete.cshtml"), model);
            return await smtpService.SendAsync(new MailMessage
            {
                Subject = $"Scan on \"{model.Project.Name}\" by {model.Scanner.Name} completed",
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