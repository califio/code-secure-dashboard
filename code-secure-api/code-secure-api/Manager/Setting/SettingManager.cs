using CodeSecure.Authentication.OpenIdConnect;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Setting;

public class SettingManager(
    AppDbContext context) : ISettingManager
{
    private static AppSetting? appSetting;
    private static MailSetting? mailSetting;
    private static MailAlertSetting? mailAlertSetting;
    private static JiraSetting? jiraSetting;
    private static TeamsSetting? teamsSetting;
    private static SLA? slaSastSetting;
    private static SLA? slaScaSetting;

    public async Task<AppSetting> AppSettingAsync()
    {
        if (appSetting == null)
        {
            appSetting = await CurrentAppSettingAsync();
        }

        Application.Setting = appSetting;
        return appSetting;
    }

    public async Task<AppSetting> UpdateAppSettingAsync(AppSetting setting)
    {
        var config = await context.AppSettings
            .OrderBy(record => record.Id)
            .FirstOrDefaultAsync();
        if (config == null)
        {
            config = new AppSettings
            {
                Id = Guid.NewGuid(),
                MailSetting = JSONSerializer.Serialize(setting.MailSetting),
                AuthSetting = JSONSerializer.Serialize(setting.AuthSetting),
                SlaScaSetting = JSONSerializer.Serialize(setting.SlaScaSetting),
                SlaSastSetting = JSONSerializer.Serialize(setting.SlaSastSetting),
            };
            context.AppSettings.Add(config);
            await context.SaveChangesAsync();
        }
        else
        {
            config.MailSetting = JSONSerializer.Serialize(setting.MailSetting);
            config.AuthSetting = JSONSerializer.Serialize(setting.AuthSetting);
            config.SlaSastSetting = JSONSerializer.Serialize(setting.SlaSastSetting);
            config.SlaScaSetting = JSONSerializer.Serialize(setting.SlaScaSetting);
            context.AppSettings.Update(config);
            await context.SaveChangesAsync();
        }

        appSetting = await CurrentAppSettingAsync();
        Application.Setting = appSetting;
        return await AppSettingAsync();
    }

    public async Task<SLA> GetSlaSastSettingAsync()
    {
        if (slaSastSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            slaSastSetting = JSONSerializer.DeserializeOrDefault(setting.SlaSastSetting, new SLA());
        }

        return slaSastSetting;
    }

    public async Task<SLA> GetSlaScaSettingAsync()
    {
        if (slaScaSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            slaScaSetting = JSONSerializer.DeserializeOrDefault(setting.SlaScaSetting, new SLA());
        }

        return slaScaSetting;
    }
    
    public async Task<JiraSetting> GetJiraSettingAsync()
    {
        if (jiraSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            jiraSetting = JSONSerializer.DeserializeOrDefault(setting.JiraSetting, new JiraSetting());
        }
        return jiraSetting;
    }

    public async Task UpdateJiraSettingAsync(JiraSetting request)
    {
        var setting = await GetAppSettingsAsync();
        setting.JiraSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        jiraSetting = request;
    }

    public async Task<MailSetting> GetMailSettingAsync()
    {
        if (mailSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            mailSetting = JSONSerializer.DeserializeOrDefault(setting.MailSetting, new MailSetting());
        }

        return mailSetting;
    }

    public async Task UpdateMailSettingAsync(MailSetting request)
    {
        var setting = await GetAppSettingsAsync();
        setting.MailSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        mailSetting = request;
    }

    public async Task<MailAlertSetting> GetMailAlertSettingAsync()
    {
        if (mailAlertSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            mailAlertSetting = JSONSerializer.DeserializeOrDefault(setting.MailAlertSetting, new MailAlertSetting());
        }

        return mailAlertSetting;
    }

    public async Task UpdateMailAlertSettingAsync(MailAlertSetting request)
    {
        var setting = await GetAppSettingsAsync();
        setting.MailAlertSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        mailAlertSetting = request;
    }

    public async Task<TeamsSetting> GetTeamsSettingAsync()
    {
        if (teamsSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            teamsSetting = JSONSerializer.DeserializeOrDefault(setting.TeamsSetting, new TeamsSetting());
        }

        return teamsSetting;
    }

    public async Task UpdateTeamsSettingAsync(TeamsSetting request)
    {
        var setting = await GetAppSettingsAsync();
        setting.TeamsSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        teamsSetting = request;
    }

    private async Task<AppSetting> CurrentAppSettingAsync()
    {
        var config = await GetAppSettingsAsync();
        return new AppSetting
        {
            MailSetting = JSONSerializer.DeserializeOrDefault(config.MailSetting,
                new MailSetting()),
            AuthSetting = JSONSerializer.DeserializeOrDefault(config.AuthSetting,
                new AuthSetting
                {
                    OpenIdConnectSetting = new OpenIdConnectSetting()
                }),
            SlaSastSetting = JSONSerializer.DeserializeOrDefault(config.SlaSastSetting,
                new SLA()),
            SlaScaSetting = JSONSerializer.DeserializeOrDefault(config.SlaScaSetting,
                new SLA()),
            TeamsNotificationSetting = JSONSerializer.DeserializeOrDefault(config.TeamsSetting,
                new TeamsSetting()),
            JiraSetting = JSONSerializer.DeserializeOrDefault(config.JiraSetting, new JiraSetting()),
        };
    }

    private async Task<AppSettings> GetAppSettingsAsync()
    {
        var config = await context.AppSettings
            .OrderBy(record => record.Id).FirstOrDefaultAsync();
        if (config == null)
        {
            config = new AppSettings
            {
                Id = Guid.NewGuid(),
                MailSetting = JSONSerializer.Serialize(new MailSetting()),
                AuthSetting = JSONSerializer.Serialize(new AuthSetting
                {
                    OpenIdConnectSetting = new OpenIdConnectSetting()
                }),
                SlaScaSetting = JSONSerializer.Serialize(new SLA
                {
                    Critical = 30,
                    High = 60,
                    Medium = 90,
                    Low = 365,
                    Info = 0
                }),
                SlaSastSetting = JSONSerializer.Serialize(new SLA
                {
                    Critical = 3,
                    High = 10,
                    Medium = 30,
                    Low = 90,
                    Info = 365
                }),
            };
            context.AppSettings.Add(config);
            await context.SaveChangesAsync();
        }

        return config;
    }
}