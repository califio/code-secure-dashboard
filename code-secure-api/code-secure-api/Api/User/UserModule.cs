using CodeSecure.Api.User.Service;

namespace CodeSecure.Api.User;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IUserService, DefaultUserService>();
        return builder;
    }
}