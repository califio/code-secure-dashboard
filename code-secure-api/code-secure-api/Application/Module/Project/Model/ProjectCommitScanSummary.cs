
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;


public record ProjectCommitScanSummary
{
    public required Guid CommitId { get; set; }
    public required CommitType Type { get; set; }
    public required string Title { get; set; }
    public required string Branch { get; set; }
    public required string? TargetBranch { get; set; }
    public required bool IsDefault { get; set; }
    public required List<ProjectScanSummary> Scans { get; set; }
    
}