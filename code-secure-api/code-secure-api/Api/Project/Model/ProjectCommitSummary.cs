using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectCommitSummary
{
    public required Guid CommitId { get; set; }
    public required string Branch { get; set; }
    public required CommitType Action { get; set; }
    public required string? TargetBranch { get; set; }
    public required bool IsDefault { get; set; }
}