using CodeSecure.Enum;

namespace CodeSecure.Database.Entity;

public class Findings : BaseEntity
{
    public required string Identity { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? Category { get; init; }
    public string? Recommendation { get; init; }
    public required FindingStatus Status { get; set; }
    public required FindingSeverity Severity { get; init; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? FixedAt { get; set; }
    public DateTime? FixDeadline { get; set; }
    public string? RuleId { get; init; }
    public string? Location { get; init; }
    public string? Snippet { get; init; }
    public int? StartLine { get; init; }
    public int? EndLine { get; init; }
    public int? StartColumn { get; init; }
    public int? EndColumn { get; init; }
    public required Guid ProjectId { get; init; }
    public Projects? Project { get; init; }
    public required Guid ScannerId { get; init; }
    public Scanners? Scanner { get; init; }
}