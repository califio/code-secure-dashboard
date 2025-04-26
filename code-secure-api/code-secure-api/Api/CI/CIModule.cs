using CodeSecure.Core;

namespace CodeSecure.Api.CI;

public class CiModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICiAuthorize, CiAuthorize>();
        return builder;
    }
}