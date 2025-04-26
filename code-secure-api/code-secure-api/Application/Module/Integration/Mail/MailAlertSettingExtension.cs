using CodeSecure.Application.Module.Setting;
using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Integration.Mail;

public static class MailAlertSettingExtension
{
    private static MailAlertSetting? setting;

    public static async Task<MailAlertSetting> GetMailAlertSettingAsync(this AppDbContext context)
    {
        if (setting == null)
        {
            var appSettings = await context.GetAppSettingsAsync();
            setting = JSONSerializer.DeserializeOrDefault(appSettings.MailAlertSetting, new MailAlertSetting());
        }
        return setting with { };
    }

    public static async Task<bool> UpdateMailAlertSettingAsync(this AppDbContext context, MailAlertSetting request)
    {
        var appSettings = await context.GetAppSettingsAsync();
        appSettings.MailAlertSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(appSettings);
        await context.SaveChangesAsync();
        setting = request;
        return true;
    }
}