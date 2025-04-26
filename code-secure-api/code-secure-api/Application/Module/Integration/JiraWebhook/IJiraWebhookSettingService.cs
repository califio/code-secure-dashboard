namespace CodeSecure.Application.Module.Integration.JiraWebhook;

public interface IJiraWebhookSettingService
{
    Task<JiraWebhookSetting> GetSettingAsync();
    Task<bool> UpdateSettingAsync(JiraWebhookSetting setting);
}

public class JIraWebhookSettingService(AppDbContext context) : IJiraWebhookSettingService
{
    public async Task<JiraWebhookSetting> GetSettingAsync()
    {
        return await context.GetJiraWebhookSettingAsync();
    }

    public async Task<bool> UpdateSettingAsync(JiraWebhookSetting setting)
    {
        return await context.UpdateJiraWebhookSettingAsync(setting);
    }
}