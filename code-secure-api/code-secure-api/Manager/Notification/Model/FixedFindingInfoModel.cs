using CodeSecure.Enum;

namespace CodeSecure.Manager.Notification.Model;

public class FixedFindingInfoModel
{
    public required string ScanName { get; set; }
    public required string CommitUrl { get; set; }
    public required string? MergeRequestUrl { get; set; }
    public required GitAction Action { get; set; }
    public required string CommitBranch { get; set; }
    public required string? TargetBranch { get; set; }
    public required string ProjectName { get; set; }
    public required List<FindingModel> Findings { get; set; }
    public required string FixedFindingUrl { get; set; }
}