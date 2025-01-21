using CodeSecure.Manager.Statistic.Model;

namespace CodeSecure.Manager.Statistic;

public interface IStatisticManager
{
    Task<SeveritySeries> SeveritySastAsync(Guid? projectId = null);
    Task<SeveritySeries> SeverityScaAsync(Guid? projectId = null);
    Task<StatusSeries> StatusSastAsync(Guid? projectId = null);
    Task<StatusSeries> StatusScaAsync(Guid? projectId = null);
    Task<List<TopFinding>> TopSastFindingAsync(Guid? projectId = null, int top = 10);
    Task<List<TopDependency>> TopDependenciesAsync(Guid? projectId = null, int top = 10);
}