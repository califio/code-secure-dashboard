using CodeSecure.Api.Admin.CIToken.Service;

namespace CodeSecure.Api.Admin.CIToken;

public class CiTokenModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICiTokenService, DefaultCiTokenService>();
        return builder;
    }
}