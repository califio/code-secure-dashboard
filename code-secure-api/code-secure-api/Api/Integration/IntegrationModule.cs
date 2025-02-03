using CodeSecure.Api.Integration.Service;

namespace CodeSecure.Api.Integration;

public class IntegrationModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IIntegrationService, IntegrationService>();
        return builder;
    }
}