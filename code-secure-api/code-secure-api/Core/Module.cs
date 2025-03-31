namespace CodeSecure.Core;

public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection builder);
}

public static class ModuleExtensions
{
    public static IServiceCollection AddAppModules(this IServiceCollection services)
    {
        var modules = DiscoverServices();
        foreach (var module in modules) module.RegisterModule(services);
        return services;
    }

    private static IEnumerable<IModule> DiscoverServices()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}