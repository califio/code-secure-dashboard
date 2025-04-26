using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Setting;

public static class SmtpSettingExtension
{
    private static SmtpSetting? smtpSetting;

    public static async Task<SmtpSetting> GetSmtpSettingAsync(this AppDbContext context)
    {
        if (smtpSetting != null) return smtpSetting;
        var setting = await context.GetAppSettingsAsync();
        smtpSetting = JSONSerializer.DeserializeOrDefault(setting.MailSetting, new SmtpSetting());
        return smtpSetting with { };
    }

    public static async Task UpdateSmtpSettingAsync(this AppDbContext context, SmtpSetting request)
    {
        var currentSetting = await GetSmtpSettingAsync(context);
        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = currentSetting.Password;
        }

        var setting = await context.GetAppSettingsAsync();
        setting.MailSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        smtpSetting = request;
    }
}