using CodeSecure.Api.Dashboard.Mode;
using CodeSecure.Api.Dashboard.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Dashboard;

public class DashboardController(IDashboardService dashboardService) : BaseController
{
    [HttpGet]
    [Route("sast")]
    public async Task<SastStatistic> SastStatistic()
    {
        return await dashboardService.SastStatisticAsync();
    }

    [HttpGet]
    [Route("sca")]
    public async Task<ScaStatistic> ScaStatistic()
    {
        return await dashboardService.ScaStatisticAsync();
    }
}