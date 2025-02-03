using CodeSecure.Api.Token.Service;

namespace CodeSecure.Api.Token;

public class TokenModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ITokenService, DefaultTokenService>();
        return builder;
    }
}