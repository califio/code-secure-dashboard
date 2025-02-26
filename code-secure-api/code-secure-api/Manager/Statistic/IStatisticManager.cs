using CodeSecure.Manager.Statistic.Model;

namespace CodeSecure.Manager.Statistic;

public interface IStatisticManager
{
    Task<SeveritySeries> SeveritySastAsync(StatisticFilter filter);
    Task<SeveritySeries> SeverityScaAsync(StatisticFilter filter);
    Task<StatusSeries> StatusSastAsync(StatisticFilter filter);
    Task<StatusSeries> StatusScaAsync(StatisticFilter filter);
    Task<List<TopFinding>> TopSastFindingAsync(StatisticFilter filter, int top = 10);
    Task<List<TopDependency>> TopDependenciesAsync(StatisticFilter filter, int top = 10);
}