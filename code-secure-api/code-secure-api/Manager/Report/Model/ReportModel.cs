using CodeSecure.Database.Entity;
using CodeSecure.Enum;

namespace CodeSecure.Manager.Report.Model;

public record ReportModel
{
    public required SourceType SourceType { get; set; }
    public required string RepoName { get; set; }
    public required string RepoUrl { get; set; }
    public required string CommitTitle { get; set; }
    public required string CommitSha { get; set; }
    public required string CommitBranch { get; set; }
    public required string? TargetBranch { get; set; }
    public string? MergeRequestId { get; set; }
    public required DateTime Time { get; set; }
    public required List<ScannerModel> Scanners { get; set; }
    public required List<FindingModel> Findings { get; set; }
}