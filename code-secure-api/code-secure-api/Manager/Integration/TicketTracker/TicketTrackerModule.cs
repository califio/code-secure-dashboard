using CodeSecure.Manager.Integration.TicketTracker.Jira;

namespace CodeSecure.Manager.Integration.TicketTracker;

public class TicketTrackerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IJiraManager, JiraManager>();
        builder.AddScoped<JiraTicketTracker>();
        builder.AddScoped<ITicketTrackerManager, TicketTrackerManager>();
        return builder;
    }
}