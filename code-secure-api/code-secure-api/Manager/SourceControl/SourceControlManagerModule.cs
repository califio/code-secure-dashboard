namespace CodeSecure.Manager.SourceControl;

public class SourceControlManagerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ISourceControlManager, SourceControlManager>();
        return builder;
    }
}