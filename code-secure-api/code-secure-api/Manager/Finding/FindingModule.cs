namespace CodeSecure.Manager.Finding;

public class FindingModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IFindingManager, FindingManager>();
        return builder;
    }
}