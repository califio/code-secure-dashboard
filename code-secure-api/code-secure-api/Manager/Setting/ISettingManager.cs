namespace CodeSecure.Manager.Setting;

public interface ISettingManager
{
    Task<AuthSetting> GetAuthSettingAsync();
    Task UpdateAuthSettingAsync(AuthSetting request);
    Task<SLA> GetSlaSastSettingAsync();
    Task UpdateSlaSastSettingAsync(SLA request);
    Task<SLA> GetSlaScaSettingAsync();
    Task UpdateSlaScaSettingAsync(SLA request);
    
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