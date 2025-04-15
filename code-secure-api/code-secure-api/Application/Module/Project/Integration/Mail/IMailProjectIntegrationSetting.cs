using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Integration.Mail;

public interface IMailProjectIntegrationSetting
{
    Task<Result<MailProjectAlertSetting>> GetSettingAsync(Guid projectId);
    Task<Result<bool>> UpdateSettingAsync(Guid projectId, MailProjectAlertSetting request);
}

public class MailProjectIntegrationSetting(AppDbContext context) : IMailProjectIntegrationSetting
{
    public async Task<Result<MailProjectAlertSetting>> GetSettingAsync(Guid projectId)
    {
        var projectSetting = await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == projectId);
        if (projectSetting == null) return Result.Fail("Project not found");
        return projectSetting.GetMailAlertSetting();
    }

    public async Task<Result<bool>> UpdateSettingAsync(Guid projectId, MailProjectAlertSetting request)
    {
        var projectSetting = await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == projectId);
        if (projectSetting == null) return Result.Fail("Project not found");
        projectSetting.MailSetting = JSONSerializer.Serialize(request);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
        return true;
    }
}