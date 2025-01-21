using CodeSecure.Api.Admin.Config.Service;

namespace CodeSecure.Api.Admin.Config;

public class ConfigModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IConfigService, DefaultConfigService>();
        return builder;
    }
}