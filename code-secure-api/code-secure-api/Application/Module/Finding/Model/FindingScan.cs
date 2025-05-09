using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding.Model;

public record FindingScan
{
    public required Guid ScanId { get; set; }
    public required string Branch { get; set; }
    public required string CommitHash { get; set; }
    public required FindingStatus Status { get; set; }
    public required CommitType Action { get; set; }
    public required string? TargetBranch { get; set; }
    public required bool IsDefault { get; set; }
}