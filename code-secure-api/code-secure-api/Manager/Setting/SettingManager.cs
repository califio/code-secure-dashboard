using CodeSecure.Authentication.OpenIdConnect;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Setting;

public class SettingManager(AppDbContext context) : ISettingManager
{
    private static AuthSetting? authSetting;
    private static MailSetting? mailSetting;
    private static MailAlertSetting? mailAlertSetting;
    private static JiraSetting? jiraSetting;
    private static TeamsSetting? teamsSetting;
    private static SLA? slaSastSetting;
    private static SLA? slaScaSetting;

    public async Task<AuthSetting> GetAuthSettingAsync()
    {
        if (authSetting == null)
        {
            var setting = await GetAppSettingsAsync();
            authSetting = JSONSerializer.DeserializeOrDefault(setting.AuthSetting, new AuthSetting());
        }

        return authSetting;
    }

    public async Task UpdateAuthSettingAsync(AuthSetting request)
    {
        var setting = await GetAppSettingsAsync();
        setting.AuthSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        authSetting = request;
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

    public async Task UpdateSlaSastSettingAsync(SLA request)
    {
        var setting = await GetAppSettingsAsync();
        setting.SlaSastSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        slaSastSetting = request;
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

    public async Task UpdateSlaScaSettingAsync(SLA request)
    {
        var setting = await GetAppSettingsAsync();
        setting.SlaScaSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        slaScaSetting = request;
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