using CodeSecure.Core.Enum;

namespace CodeSecure.Core.Entity;

public class Scans : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required ScanStatus Status { get; set; }
    public required string JobUrl { get; set; }
    public required DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public required Guid CommitId { get; set; }
    public GitCommits? Commit { get; set; }
    public string? LastCommitHash { get; set; }
    public required Guid ProjectId { get; set; }
    public Projects? Project { get; set; }
    public required Guid ScannerId { get; set; }
    public Scanners? Scanner { get; set; }
    public Guid? ContainerId { get; set; }
    public Containers? Container { get; set; }
}