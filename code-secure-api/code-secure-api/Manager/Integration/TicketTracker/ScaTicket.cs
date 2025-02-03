using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Integration.TicketTracker;

public class ScaTicket
{
    public required string Location { get; set; }
    public required Projects Project { get; set; }
    public required Packages Package { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
}