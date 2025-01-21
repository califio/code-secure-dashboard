using CodeSecure.Api.Dashboard.Mode;
using CodeSecure.Manager.Statistic;

namespace CodeSecure.Api.Dashboard.Service;

public class DefaultDashboardService(IStatisticManager statisticManager) : IDashboardService
{
    public async Task<SastStatistic> SastStatisticAsync()
    {
        return new SastStatistic
        {
            Severity = await statisticManager.SeveritySastAsync(),
            Status = await statisticManager.StatusSastAsync(),
            TopFindings = await statisticManager.TopSastFindingAsync(top: 10)
        };
    }

    public async Task<ScaStatistic> ScaStatisticAsync()
    {
        return new ScaStatistic
        {
            Severity = await statisticManager.SeverityScaAsync(),
            Status = await statisticManager.StatusScaAsync(),
            TopDependencies = await statisticManager.TopDependenciesAsync(top: 10)
        };
    }
}