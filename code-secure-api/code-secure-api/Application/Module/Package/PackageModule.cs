using CodeSecure.Core;

namespace CodeSecure.Application.Module.Package;

public class PackageModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IPackageService, PackageService>();
        return builder;
    }
}