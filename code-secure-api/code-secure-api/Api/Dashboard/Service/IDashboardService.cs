using CodeSecure.Api.Dashboard.Mode;

namespace CodeSecure.Api.Dashboard.Service;

public interface IDashboardService
{
    Task<SastStatistic> SastStatisticAsync(DateTime? from = null, DateTime? to = null);
    Task<ScaStatistic> ScaStatisticAsync(DateTime? from = null, DateTime? to = null);
}