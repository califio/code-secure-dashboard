using CodeSecure.Application.Module.Setting;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Integration.Teams;

public static class TeamsAlertSettingExtension
{
    private static TeamsAlertSetting? setting;
    public static async Task<TeamsAlertSetting> GetTeamsAlertSettingAsync(this AppDbContext context)
    {
        if (setting == null)
        {
            var appSettings = await context.GetAppSettingsAsync();
            setting = JSONSerializer.DeserializeOrDefault(appSettings.TeamsSetting, new TeamsAlertSetting());
        }

        return setting with { };
    }
    
    public static async Task<bool> UpdateTeamsAlertSettingAsync(this AppDbContext context, TeamsAlertSetting request)
    {
        var currentSetting = await GetTeamsAlertSettingAsync(context);
        if (string.IsNullOrEmpty(request.Webhook))
        {
            request.Webhook = currentSetting.Webhook;
        }

        var appSettings = await context.GetAppSettingsAsync();
        appSettings.TeamsSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(appSettings);
        await context.SaveChangesAsync();
        setting = request;
        return true;
    }
}