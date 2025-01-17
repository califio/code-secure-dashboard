using CodeSecure.Enum;

namespace CodeSecure.Database.Entity;

public class ProjectCommits : BaseEntity
{
    public required Guid ProjectId { get; set; }
    public required bool IsDefault { get; set; }
    public required string Branch { get; set; }

    public required GitAction Action { get; set; }

    // use when merge request
    public string? TargetBranch { get; set; }

    //
    public Projects? Project { get; set; }
}