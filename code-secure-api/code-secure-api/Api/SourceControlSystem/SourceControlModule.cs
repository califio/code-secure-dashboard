using CodeSecure.Api.SourceControlSystem.Service;

namespace CodeSecure.Api.SourceControlSystem;

public class SourceControlModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ISourceControlService, SourceControlService>();
        return builder;
    }
}