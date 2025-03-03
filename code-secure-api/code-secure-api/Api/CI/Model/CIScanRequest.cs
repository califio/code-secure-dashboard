using System.ComponentModel.DataAnnotations;
using CodeSecure.Enum;
using CodeSecure.Validator;

namespace CodeSecure.Api.CI.Model;

public record CiScanRequest
{
    [Required] public required SourceType Source { get; set; }
    [Required] public required string RepoId { get; set; }

    [Required] 
    [HttpUrl]
    public required string RepoUrl { get; set; }

    [Required] public required string RepoName { get; set; }

    [Required] public required CommitType CommitType { get; set; }

    [Required] public required string ScanTitle { get; set; }

    public required string CommitBranch { get; set; }

    [Required] public required string CommitHash { get; set; }

    public string? TargetBranch { get; set; }
    public string? MergeRequestId { get; set; }

    [Required] public required string Scanner { get; set; }

    [Required] public required ScannerType Type { get; set; }

    [Required] public required string JobUrl { get; set; }

    public required bool IsDefault { get; set; }
    public string? ContainerImage { get; set; }
}