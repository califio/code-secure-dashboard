using CodeSecure.Application.Services;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public class AlertProjectWithoutMemberMail(IRazorRender render, ISmtpService smtpService): IAlertProjectWithoutMember
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertProjectWithoutMemberModel model)
    {
        try
        {
            var content = await render.RenderAsync(Path.Combine("Resources", "Templates", "AlertProjectWithoutMember.cshtml"), model);
            return await smtpService.SendAsync(new MailMessage
            {
                Subject = $"Action Required: Add at least one member to {model.Project.Name} to receive notifications",
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