using CodeSecure.Api.Role.Service;

namespace CodeSecure.Api.Role;

public class RoleModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IRoleService, DefaultRoleService>();
        return builder;
    }
}