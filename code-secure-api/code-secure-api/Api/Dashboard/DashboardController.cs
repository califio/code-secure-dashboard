using CodeSecure.Application;
using CodeSecure.Application.Module.Stats;
using CodeSecure.Application.Module.Stats.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Dashboard;

public class DashboardController(AppDbContext context)
    : BaseController
{
    [HttpPost]
    [Route("sast")]
    public async Task<SastStatistic> SastStatistic(StatisticFilter filter)
    {
        return new SastStatistic
        {
            Severity = await context.StatsSastFindingBySeverityAsync(filter),
            Status = await context.StatsSastFindingByStatusAsync(filter),
            TopFindings = await context.StatsTopSastFindingAsync(filter, top: 10)
        };
    }

    [HttpPost]
    [Route("sca")]
    public async Task<ScaStatistic> ScaStatistic(StatisticFilter filter)
    {
        return new ScaStatistic
        {
            Severity = await context.StatsPackageProjectBySeverityAsync(filter),
            Status = await context.StatsPackageProjectByStatusAsync(filter),
            TopDependencies = await context.StatsTopDependenciesAsync(filter, top: 10)
        };
    }
}