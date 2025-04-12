using FluentResults;

namespace CodeSecure.Application.Module.Integration.JiraWebhook;

public interface IJiraWebhookSettingService
{
    Task<JiraWebhookSetting> GetSettingAsync();
    Task<Result<bool>> UpdateSettingAsync(JiraWebhookSetting setting);
    Task<Result<bool>> TestConnectionAsync();
}

public class JIraWebhookSettingService(AppDbContext context) : IJiraWebhookSettingService
{
    public async Task<JiraWebhookSetting> GetSettingAsync()
    {
        return await context.GetJiraWebhookSettingAsync();
    }

    public async Task<Result<bool>> UpdateSettingAsync(JiraWebhookSetting setting)
    {
        return await context.UpdateJiraWebhookSettingAsync(setting);
    }

    public async Task<Result<bool>> TestConnectionAsync()
    {
        var setting = await context.GetJiraWebhookSettingAsync();
        // todo: need implement
        return true;
    }
}