using CodeSecure.Application.Helpers;
using CodeSecure.Application.Services;
using CodeSecure.Core.Entity;
using FluentResults;

namespace CodeSecure.Application.Module.Mail;

public record MailRemoveUserFromProjectModel
{
    public required string Username { get; init; }
    public required Projects Project { get; init; }

    public string ProjectUrl()
    {
        return FrontendUrlHelper.ProjectUrl(Project.Id);
    }
}

public interface IMailRemoveUserFromProject
{
    Task<Result<bool>> SendAsync(string receiver, MailRemoveUserFromProjectModel model);
}

public class MailRemoveUserFromProject(ISmtpService smtpService, IRazorRender render) : IMailRemoveUserFromProject
{
    public async Task<Result<bool>> SendAsync(string receiver, MailRemoveUserFromProjectModel model)
    {
        var content =
            await render.RenderAsync(Path.Combine("Resources", "Templates", "MailRemoveUserFromProject.cshtml"), model);
        return await smtpService.SendAsync(new MailMessage
        {
            Subject = $"You have been removed from project {model.Project.Name}",
            Receivers = [receiver],
            Content = content,
        });
    }
}