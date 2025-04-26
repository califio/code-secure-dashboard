using CodeSecure.Authentication.OpenIdConnect;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Setting;

public static class SettingExtension
{
    
    public static async Task<AppSettings> GetAppSettingsAsync(this AppDbContext context)
    {
        var config = await context.AppSettings.OrderBy(record => record.Id).FirstOrDefaultAsync();
        if (config != null) return config;
        config = new AppSettings
        {
            Id = Guid.NewGuid(),
            MailSetting = JSONSerializer.Serialize(new SmtpSetting()),
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
        return config;
    }
    
}