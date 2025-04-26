using CodeSecure.Core;

namespace CodeSecure.Application.Module.User;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IUserService, UserService>();
        return builder;
    }
}