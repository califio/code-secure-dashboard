namespace CodeSecure.Manager.Package;

public class PackageManagerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IPackageManager, PackageManager>();
        return builder;
    }
}