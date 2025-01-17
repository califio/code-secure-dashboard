using CodeSecure.Api.Auth.Service;

namespace CodeSecure.Api.Auth;

public class AuthModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IAuthService, DefaultAuthService>();
        return builder;
    }
}