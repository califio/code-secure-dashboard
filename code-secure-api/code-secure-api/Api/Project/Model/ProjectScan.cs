using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectScan
{
    public required Guid Id { get; set; }
    public required GitAction GitAction { get; set; }
    public required string? Metadata { get; set; }
    public required Guid CommitId { get; set; }

    public required string CommitTitle { get; set; }
    public required string? CommitBranch { get; set; }
    public required string? TargetBranch { get; set; }
    public required Guid ScannerId { get; set; }
    public required string Scanner { get; set; }
    public required ScannerType Type { get; set; }
    public required ScanStatus Status { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime? CompletedAt { get; set; }
    public required int SeverityCritical { get; set; }
    public required int SeverityHigh { get; set; }
    public required int SeverityMedium { get; set; }
    public required int SeverityLow { get; set; }
    public required int SeverityInfo { get; set; }
    public required string JobUrl { get; set; }
}