using CodeSecure.Core;

namespace CodeSecure.Application.Module.SourceControl;

public class SourceControlModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ISourceControlService, SourceControlService>();
        return builder;
    }
}