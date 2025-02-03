namespace CodeSecure.Manager.Setting;

public class SettingModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ISettingManager, SettingManager>();
        builder.AddScoped<JiraSetting>(sp =>
        {
            var settingManager = sp.GetRequiredService<ISettingManager>();
            return settingManager.GetJiraSettingAsync().Result;
        });
        builder.AddScoped<TeamsSetting>(sp =>
        {
            var settingManager = sp.GetRequiredService<ISettingManager>();
            return settingManager.GetTeamsSettingAsync().Result;
        });
        builder.AddScoped<MailSetting>(sp =>
        {
            var settingManager = sp.GetRequiredService<ISettingManager>();
            return settingManager.GetMailSettingAsync().Result;
        });
        builder.AddScoped<MailAlertSetting>(sp =>
        {
            var settingManager = sp.GetRequiredService<ISettingManager>();
            return settingManager.GetMailAlertSettingAsync().Result;
        });
        return builder;
    }
}