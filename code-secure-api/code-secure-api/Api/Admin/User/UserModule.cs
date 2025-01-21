using CodeSecure.Api.Admin.User.Service;

namespace CodeSecure.Api.Admin.User;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IUserService, DefaultUserService>();
        return builder;
    }
}