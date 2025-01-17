using CodeSecure.Api.Admin.Config.Model;
using CodeSecure.Database.Metadata;

namespace CodeSecure.Api.Admin.Config.Service;

public interface IConfigService
{
    Task<AuthInfo> GetAuthInfoAsync();
    Task<MailSetting> GetMailSettingAsync();
    Task<MailSetting> UpdateMailSettingAsync(MailSettingRequest request);
    Task<TeamsNotificationSetting> GetTeamsNotificationSettingAsync();
    Task<TeamsNotificationSetting> UpdateTeamsNotificationSettingAsync(TeamsNotificationSettingRequest request);
    Task<AuthSetting> GetAuthSettingAsync();
    Task<AuthSetting> UpdateAuthSettingAsync(AuthSetting request);

    Task<SlaSetting> GetSlaSettingAsync();
    Task<SlaSetting> UpdateSlaSettingAsync(SlaSetting request);
}