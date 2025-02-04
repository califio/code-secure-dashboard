using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Integration.TicketTracker;

public class SastTicket
{
    public required string Commit { get; set; }
    public required Projects Project { get; set; }
    public required Findings Finding { get; set; }
    public required Scanners Scanner { get; set; }
}