using CodeSecure.Api.Dependency.Service;

namespace CodeSecure.Api.Dependency;

public class DependencyModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IDependencyService, DefaultDependencyService>();
        return builder;
    }
}