using CodeSecure.Api.Setting.Model;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Api.Setting.Service;

public interface ISettingService
{
    Task<AuthSetting> GetAuthSettingAsync();
    Task<AuthSetting> UpdateAuthSettingAsync(AuthSetting request);

    Task<SlaSetting> GetSlaSettingAsync();
    Task<SlaSetting> UpdateSlaSettingAsync(SlaSetting request);

    // mail
    Task<MailSetting> GetMailSettingAsync();
    Task UpdateMailSettingAsync(MailSetting setting);
}