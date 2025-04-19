using CodeSecure.Core.Enum;

namespace CodeSecure.Core.Entity;

public class GitCommits : BaseEntity
{
    public string? CommitHash { get; set; }
    public string? CommitTitle { get; set; }
    public required CommitType Type { get; set; }
    public required string Branch { get; set; }

    public required bool IsDefault { get; set; }

    // use when merge request
    public string? TargetBranch { get; set; }

    public string? MergeRequestId { get; set; }

    // project
    public required Guid ProjectId { get; set; }
    public Projects? Project { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is GitCommits commit)
        {
            return Id == commit.Id;
        }
        return false;
    }
    public override int GetHashCode() => Id.GetHashCode();
}