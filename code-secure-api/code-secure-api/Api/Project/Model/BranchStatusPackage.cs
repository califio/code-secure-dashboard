using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record BranchStatusPackage : BranchStatus
{
    public required PackageStatus Status { get; set; }
}