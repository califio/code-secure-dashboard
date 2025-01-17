using CodeSecure.Api.Dashboard.Mode;

namespace CodeSecure.Api.Dashboard.Service;

public interface IDashboardService
{
    Task<SastStatistic> SastStatisticAsync();
    Task<ScaStatistic> ScaStatisticAsync();
}