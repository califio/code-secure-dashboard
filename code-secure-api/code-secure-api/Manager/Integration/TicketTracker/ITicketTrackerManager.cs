using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Integration.TicketTracker;

public interface ITicketTrackerManager
{
    public Task CreateTicketAsync(Findings finding);
    public Task CreateTicketAsync(SastTicket request);
    public Task CreateTicketAsync(ScaTicket request); 
}