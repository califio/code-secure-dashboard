namespace CodeSecure.Manager.Finding;

public class FindingManagerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IFindingManager, FindingManager>();
        return builder;
    }
}