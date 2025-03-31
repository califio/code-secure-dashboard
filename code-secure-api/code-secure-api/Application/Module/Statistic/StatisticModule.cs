using CodeSecure.Core;

namespace CodeSecure.Application.Module.Statistic;

public class StatisticModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IStatsSastFinding, StatsSastFinding>();
        builder.AddScoped<IStatsPackageProject, StatsPackageProject>();
        return builder;
    }
}