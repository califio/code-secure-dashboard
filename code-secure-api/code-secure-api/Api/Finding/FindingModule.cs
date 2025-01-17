using CodeSecure.Api.Finding.Service;

namespace CodeSecure.Api.Finding;

public class FindingModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IFindingService, DefaultFindingService>();
        return builder;
    }
}