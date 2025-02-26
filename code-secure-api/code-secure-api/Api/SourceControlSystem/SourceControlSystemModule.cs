using CodeSecure.Api.SourceControlSystem.Service;

namespace CodeSecure.Api.SourceControlSystem;

public class SourceControlSystemModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ISourceControlSystemService, SourceControlSystemService>();
        return builder;
    }
}