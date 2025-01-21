using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ProjectId), nameof(ContainerId))]
public class ProjectContainers
{
    public required Guid ProjectId { get; set; }
    public required Guid ContainerId { get; set; }
    public Projects? Project { get; set; }
    public Containers? Container { get; set; }
}