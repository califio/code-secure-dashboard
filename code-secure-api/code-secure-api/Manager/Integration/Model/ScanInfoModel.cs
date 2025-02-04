using CodeSecure.Enum;

namespace CodeSecure.Manager.Integration.Model;

public record ScanInfoModel
{
    public required Guid ProjectId { get; set; }
    public required string ProjectUrl { get; set; }
    public required string ProjectName { get; set; }
    public required string ScanName { get; set; }
    public required string ScannerName { get; set; }
    public required string ScannerType { get; set; }
    public required string BlockStatus { get; set; }
    public required GitAction Action { get; set; }
    public required string CommitUrl { get; set; }
    public required string CommitBranch { get; set; }
    public required string? TargetBranch { get; set; }
    public required string? MergeRequestUrl { get; set; }
    public required string FindingUrl { get; set; }
    public required int NewFinding { get; set; }
    public required int NeedsTriage { get; set; }
    public required int Confirmed { get; set; }
}