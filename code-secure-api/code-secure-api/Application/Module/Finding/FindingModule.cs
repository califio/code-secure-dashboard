using CodeSecure.Core;

namespace CodeSecure.Application.Module.Finding;

public class FindingModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IFindingAuthorize, FindingAuthorize>();
        builder.AddScoped<IFindingService, FindingService>();
        return builder;
    }
}