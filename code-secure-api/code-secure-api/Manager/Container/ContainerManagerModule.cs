namespace CodeSecure.Manager.Container;

public class ContainerManagerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IContainerManager, ContainerManager>();
        return builder;
    }
}