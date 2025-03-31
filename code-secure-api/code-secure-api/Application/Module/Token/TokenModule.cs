using CodeSecure.Core;

namespace CodeSecure.Application.Module.Token;

public class TokenModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICreateTokenHandler, CreateTokenHandler>();
        builder.AddScoped<IDeleteTokenHandler, DeleteTokenHandler>();
        builder.AddScoped<IListTokenHandler, ListTokenHandler>();
        return builder;
    }
}