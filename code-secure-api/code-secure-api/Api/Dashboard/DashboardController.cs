using CodeSecure.Api.Dashboard.Mode;
using CodeSecure.Manager.Statistic;
using CodeSecure.Manager.Statistic.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Dashboard;

public class DashboardController(IStatisticManager statisticManager) : BaseController
{
    [HttpPost]
    [Route("sast")]
    public async Task<SastStatistic> SastStatistic(StatisticFilter filter)
    {
        return new SastStatistic
        {
            Severity = await statisticManager.SeveritySastAsync(filter),
            Status = await statisticManager.StatusSastAsync(filter),
            TopFindings = await statisticManager.TopSastFindingAsync(filter, top: 10)
        };
    }

    [HttpPost]
    [Route("sca")]
    public async Task<ScaStatistic> ScaStatistic(StatisticFilter filter)
    {
        return new ScaStatistic
        {
            Severity = await statisticManager.SeverityScaAsync(filter),
            Status = await statisticManager.StatusScaAsync(filter),
            TopDependencies = await statisticManager.TopDependenciesAsync(filter, top: 10)
        };
    }
}