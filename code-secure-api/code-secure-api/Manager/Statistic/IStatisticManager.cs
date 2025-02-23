using CodeSecure.Manager.Statistic.Model;

namespace CodeSecure.Manager.Statistic;

public interface IStatisticManager
{
    Task<SeveritySeries> SeveritySastAsync(Guid? projectId = null, DateTime? from = null, DateTime? to = null);
    Task<SeveritySeries> SeverityScaAsync(Guid? projectId = null, DateTime? from = null, DateTime? to = null);
    Task<StatusSeries> StatusSastAsync(Guid? projectId = null, DateTime? from = null, DateTime? to = null);
    Task<StatusSeries> StatusScaAsync(Guid? projectId = null, DateTime? from = null, DateTime? to = null);
    Task<List<TopFinding>> TopSastFindingAsync(Guid? projectId = null, int top = 10, DateTime? from = null, DateTime? to = null);
    Task<List<TopDependency>> TopDependenciesAsync(Guid? projectId = null, int top = 10);
}