namespace CodeSecure.Manager.Statistic;

public class StatisticManagerModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IStatisticManager, StatisticManager>();
        return builder;
    }
}