using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Integration.TicketTracker;

public interface ITicketTracker
{
    public Task<TicketResult<Tickets>> CreateTicketAsync(SastTicket request);

    public Task<TicketResult<Tickets>> CreateTicketAsync(ScaTicket request);
}