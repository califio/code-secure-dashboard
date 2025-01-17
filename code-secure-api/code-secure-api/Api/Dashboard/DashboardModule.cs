using CodeSecure.Api.Dashboard.Service;

namespace CodeSecure.Api.Dashboard;

public class DashboardModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IDashboardService, DefaultDashboardService>();
        return builder;
    }
}