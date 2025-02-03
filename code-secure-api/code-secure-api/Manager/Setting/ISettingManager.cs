namespace CodeSecure.Manager.Setting;

public interface ISettingManager
{
    Task<AppSetting> AppSettingAsync();
    Task<AppSetting> UpdateAppSettingAsync(AppSetting setting);
    Task<SLA> GetSlaSastSettingAsync();
    Task<SLA> GetSlaScaSettingAsync();
    
    Task<MailSetting> GetMailSettingAsync();
    Task UpdateMailSettingAsync(MailSetting request);

    #region Integration
    Task<MailAlertSetting> GetMailAlertSettingAsync();
    Task UpdateMailAlertSettingAsync(MailAlertSetting request);
    Task<JiraSetting> GetJiraSettingAsync();
    Task UpdateJiraSettingAsync(JiraSetting request);
    
    Task<TeamsSetting> GetTeamsSettingAsync();
    Task UpdateTeamsSettingAsync(TeamsSetting request);
    #endregion
    
}