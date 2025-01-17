using CodeSecure.Authentication.OpenIdConnect;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Metadata;
using CodeSecure.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Setting;

public class AppSettingManager(
    AppDbContext context) : IAppSettingManager
{
    private static AppSetting? appSetting;

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

    public async Task<MailSetting> GetMailSettingAsync()
    {
        var setting = await AppSettingAsync();
        return setting.MailSetting;
    }

    public async Task UpdateMailSettingAsync(MailSetting setting)
    {
        var config = await GetAppSettingsAsync();
        config.MailSetting = JSONSerializer.Serialize(setting);
        context.AppSettings.Update(config);
        await context.SaveChangesAsync();
        if (appSetting != null)
        {
            appSetting.MailSetting = setting;
        }
    }

    public async Task<TeamsNotificationSetting> GetTeamsNotificationSettingAsync()
    {
        var setting = await AppSettingAsync();
        return setting.TeamsNotificationSetting;
    }

    public async Task<TeamsNotificationSetting> UpdateTeamsNotificationSettingAsync(TeamsNotificationSetting setting)
    {
        var config = await GetAppSettingsAsync();
        config.TeamsNotificationSetting = JSONSerializer.Serialize(setting);
        context.AppSettings.Update(config);
        await context.SaveChangesAsync();
        if (appSetting != null)
        {
            appSetting.TeamsNotificationSetting = setting;
        }

        return setting;
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
            TeamsNotificationSetting = JSONSerializer.DeserializeOrDefault(config.TeamsNotificationSetting,
                new TeamsNotificationSetting()),
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