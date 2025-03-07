using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[Index(nameof(ProjectId), nameof(PackageId), nameof(Location), IsUnique = true)]
public class ProjectPackages: BaseEntity
{
    public required Guid ProjectId { get; set; }
    public required Guid PackageId { get; set; }
    public required string Location { get; set; }
    public Guid? TicketId { get; set; }
    public Tickets? Ticket { get; set; }
    public Projects? Project { get; set; }
    public Packages? Package { get; set; }
}