using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Rule.Model;

public record RuleInfo
{
    public required string Id { get; set; }
    public required RuleStatus Status { get; set; }
    public required RuleConfidence Confidence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required Guid ScannerId { get; set; }
    public required string ScannerName { get; set; }
    public required int IncorrectFinding { get; set; }
    public required int CorrectFinding { get; set; }
    public required int UncertainFinding { get; set; }
}