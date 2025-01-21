using CodeSecure.Api.CI.Service;

namespace CodeSecure.Api.CI;

public class CiModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICiAuthorize, CiAuthorize>();
        builder.AddScoped<ICiService, DefaultCiService>();
        return builder;
    }
}