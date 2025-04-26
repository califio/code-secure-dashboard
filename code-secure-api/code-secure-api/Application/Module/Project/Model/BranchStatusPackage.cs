using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record BranchStatusPackage
{
    public required PackageStatus Status { get; set; }
    public required string? CommitHash { get; set; }
    public required string? CommitTitle { get; set; }
    public required CommitType CommitType { get; set; }
    public required string CommitBranch { get; set; }
    // use when merge request
    public required string? TargetBranch { get; set; }
    public required string? MergeRequestId { get; set; }
}