using CodeSecure.Database.Metadata;

namespace CodeSecure.Manager.Setting;

public interface IAppSettingManager
{
    Task<AppSetting> AppSettingAsync();
    Task<AppSetting> UpdateAppSettingAsync(AppSetting setting);

    Task<MailSetting> GetMailSettingAsync();
    Task UpdateMailSettingAsync(MailSetting setting);
    Task<TeamsNotificationSetting> GetTeamsNotificationSettingAsync();
    Task<TeamsNotificationSetting> UpdateTeamsNotificationSettingAsync(TeamsNotificationSetting setting);
}