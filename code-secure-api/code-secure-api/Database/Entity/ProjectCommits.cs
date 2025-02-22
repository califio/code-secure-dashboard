using CodeSecure.Enum;

namespace CodeSecure.Database.Entity;

public class ProjectCommits : BaseEntity
{
    public required Guid ProjectId { get; set; }
    public required bool IsDefault { get; set; }
    public required string Branch { get; set; }

    public required GitAction Action { get; set; }
    public string? CommitHash { get; set; }

    public string? CommitTitle { get; set; }
    // use when merge request
    public required string? TargetBranch { get; set; }
    public required string? MergeRequestId { get; set; }

    //
    public Projects? Project { get; set; }
}