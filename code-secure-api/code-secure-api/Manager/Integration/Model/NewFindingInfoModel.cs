using CodeSecure.Enum;

namespace CodeSecure.Manager.Integration.Model;

public record NewFindingInfoModel
{
    public required Guid ProjectId { get; set; }
    public required string ScanName { get; set; }
    public required string CommitUrl { get; set; }
    public required string? MergeRequestUrl { get; set; }
    public required CommitType Action { get; set; }
    public required string CommitBranch { get; set; }
    public required string? TargetBranch { get; set; }
    public required string ProjectName { get; set; }
    public required string ScannerName { get; set; }
    public required string ScannerType { get; set; }
    public required List<FindingModel> Findings { get; set; }
    public required string OpenFindingUrl { get; set; }
}