using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(RepoId), nameof(SourceControlId), IsUnique = true)]
public class Projects : BaseEntity
{
    public required string Name { get; set; }
    public required string RepoId { get; set; }
    public required string RepoUrl { get; set; }
    public required Guid SourceControlId { get; set; }
    public SourceControls? SourceControl { get; set; }
}