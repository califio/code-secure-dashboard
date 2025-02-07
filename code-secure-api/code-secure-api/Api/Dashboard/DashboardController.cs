using CodeSecure.Api.Dashboard.Mode;
using CodeSecure.Api.Dashboard.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Dashboard;

public class DashboardController(IDashboardService dashboardService) : BaseController
{
    [HttpGet]
    [Route("sast")]
    public async Task<SastStatistic> SastStatistic(DateTime? from = null, DateTime? to = null)
    {
        Console.WriteLine(from);
        Console.WriteLine(to);
        return await dashboardService.SastStatisticAsync(from, to);
    }

    [HttpGet]
    [Route("sca")]
    public async Task<ScaStatistic> ScaStatistic(DateTime? from = null, DateTime? to = null)
    {
        return await dashboardService.ScaStatisticAsync(from, to);
    }
}