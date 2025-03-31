using CodeSecure.Application.Module.Statistic;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Dashboard;

public class DashboardController(IStatsPackageProject statsPackageProject, IStatsSastFinding statsSastFinding)
    : BaseController
{
    [HttpPost]
    [Route("sast")]
    public async Task<SastStatistic> SastStatistic(StatisticFilter filter)
    {
        return new SastStatistic
        {
            Severity = await statsSastFinding.StatsSastFindingBySeverityAsync(filter),
            Status = await statsSastFinding.StatsSastFindingByStatusAsync(filter),
            TopFindings = await statsSastFinding.StatsTopSastFindingAsync(filter, top: 10)
        };
    }

    [HttpPost]
    [Route("sca")]
    public async Task<ScaStatistic> ScaStatistic(StatisticFilter filter)
    {
        return new ScaStatistic
        {
            Severity = await statsPackageProject.StatsPackageProjectBySeverityAsync(filter),
            Status = await statsPackageProject.StatsPackageProjectByStatusAsync(filter),
            TopDependencies = await statsPackageProject.StatsTopDependenciesAsync(filter, top: 10)
        };
    }
}