using CodeSecure.Api.Admin.Role.Service;

namespace CodeSecure.Api.Admin.Role;

public class RoleModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IRoleService, DefaultRoleService>();
        return builder;
    }
}