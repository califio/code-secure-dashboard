using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ProjectId), nameof(Name))]
public class ProjectEnvironmentVariables
{
    public required Guid ProjectId { get; set; }
    public Projects? Project { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
}