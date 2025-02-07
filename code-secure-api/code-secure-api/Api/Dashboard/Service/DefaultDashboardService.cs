using CodeSecure.Api.Dashboard.Mode;
using CodeSecure.Manager.Statistic;

namespace CodeSecure.Api.Dashboard.Service;

public class DefaultDashboardService(IStatisticManager statisticManager) : IDashboardService
{
    public async Task<SastStatistic> SastStatisticAsync(DateTime? from = null, DateTime? to = null)
    {
        return new SastStatistic
        {
            Severity = await statisticManager.SeveritySastAsync(null, from, to),
            Status = await statisticManager.StatusSastAsync(null, from, to),
            TopFindings = await statisticManager.TopSastFindingAsync(null, top: 10, from, to)
        };
    }

    public async Task<ScaStatistic> ScaStatisticAsync(DateTime? from = null, DateTime? to = null)
    {
        return new ScaStatistic
        {
            Severity = await statisticManager.SeverityScaAsync(null, from, to),
            Status = await statisticManager.StatusScaAsync(null, from, to),
            TopDependencies = await statisticManager.TopDependenciesAsync(top: 10)
        };
    }
}