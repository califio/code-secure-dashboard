using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ProjectId), nameof(PackageId), nameof(Location))]
public class ProjectPackages
{
    public required Guid ProjectId { get; set; }
    public required Guid PackageId { get; set; }
    public required string Location { get; set; }
    public Guid? TicketId { get; set; }
    public Tickets? Ticket { get; set; }
    public Projects? Project { get; set; }
    public Packages? Package { get; set; }
}