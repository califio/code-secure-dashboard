using CodeSecure.Api.Setting.Service;

namespace CodeSecure.Api.Setting;

public class SettingModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ISettingService, DefaultSettingService>();
        return builder;
    }
}