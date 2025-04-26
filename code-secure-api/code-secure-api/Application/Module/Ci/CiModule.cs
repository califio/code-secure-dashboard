using CodeSecure.Core;

namespace CodeSecure.Application.Module.Ci;

public class CiModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICiService, CiService>();
        return builder;
    }
}