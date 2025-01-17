using CodeSecure.Database.Metadata;

namespace CodeSecure.Manager.Setting;

public class SettingManagerModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IAppSettingManager, AppSettingManager>();
        builder.AddScoped<MailSetting>(sp =>
        {
            var service = sp.GetRequiredService<IAppSettingManager>();
            return service.GetMailSettingAsync().Result;
        });
        return builder;
    }
}