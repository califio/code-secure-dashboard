using CodeSecure.Core;

namespace CodeSecure.Application.Module.Setting;

public class SettingModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IAuthSetting, AuthSettingImpl>();
        builder.AddScoped<SmtpSetting>(sp =>
        {
            var context = sp.GetRequiredService<AppDbContext>();
            return context.GetSmtpSettingAsync().Result;
        });
        return builder;
    }
}