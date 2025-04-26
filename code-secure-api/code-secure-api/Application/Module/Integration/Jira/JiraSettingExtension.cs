using CodeSecure.Application.Module.Setting;
using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Integration.Jira;

public static class JiraSettingExtension
{
    private static JiraSetting? setting;
    public static async Task<JiraSetting> GetJiraSettingAsync(this AppDbContext context)
    {
        if (setting == null)
        {
            var appSettings = await context.GetAppSettingsAsync();
            setting = JSONSerializer.DeserializeOrDefault(appSettings.JiraSetting, new JiraSetting());
        }
        return setting with { };
    }
    
    public static async Task<bool> UpdateJiraSettingAsync(this AppDbContext context, JiraSetting request)
    {
        var currentSetting = await context.GetJiraSettingAsync();
        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = currentSetting.Password;
        }
        var appSettings = await context.GetAppSettingsAsync();
        appSettings.JiraSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(appSettings);
        await context.SaveChangesAsync();
        setting = request;
        return true;
    }
}