using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ProjectId))]
public class ProjectSettings
{
    public required Guid ProjectId { get; set; }
    public required string Metadata { get; set; }
}