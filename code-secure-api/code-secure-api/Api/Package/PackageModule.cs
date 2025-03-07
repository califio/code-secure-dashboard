using CodeSecure.Api.Package.Service;

namespace CodeSecure.Api.Package;

public class PackageModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IPackageService, PackageService>();
        return builder;
    }
}