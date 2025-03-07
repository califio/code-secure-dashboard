using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record BranchStatusFinding
{
    public required FindingStatus Status { get; set; }
}