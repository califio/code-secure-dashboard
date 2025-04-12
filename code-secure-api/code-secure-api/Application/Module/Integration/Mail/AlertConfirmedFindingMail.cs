using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertConfirmedFindingMail(ISmtpService smtpService, IRazorRender render): IAlertConfirmedFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertConfirmedFindingModel model)
    {
        try
        {
            var content = await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertConfirmedFinding.cshtml"), model);
            return await smtpService.SendAsync(new MailMessage
            {
                Subject = $"Security Alert: \"{model.Project.Name}\" project has {model.Findings.Count} issues to fix",
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