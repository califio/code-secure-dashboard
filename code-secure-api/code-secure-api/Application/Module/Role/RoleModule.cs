using CodeSecure.Core;

namespace CodeSecure.Application.Module.Role;

public class RoleModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IRoleService, RoleService>();
        return builder;
    }
}