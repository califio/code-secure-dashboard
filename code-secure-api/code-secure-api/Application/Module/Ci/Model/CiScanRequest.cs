using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Validators;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Ci.Model;

public record CiScanRequest
{
    [Required] public required SourceType Source { get; set; }
    [Required] public required string RepoId { get; set; }

    [Required] 
    [HttpUrl]
    public required string RepoUrl { get; set; }

    public string RepoName { get; set; } = string.Empty;

    [Required] public required CommitType GitAction { get; set; }

    [Required] public required string ScanTitle { get; set; }

    public required string CommitBranch { get; set; }

    [Required] public required string CommitHash { get; set; }

    public string? TargetBranch { get; set; }
    public string? MergeRequestId { get; set; }

    [Required] public required string Scanner { get; set; }

    [Required] public required ScannerType Type { get; set; }

    public string JobUrl { get; set; } = string.Empty;

    public required bool IsDefault { get; set; }
    public string? ContainerImage { get; set; }
}