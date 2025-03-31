using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Report.Model;

public record FindingModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string? Recommendation { get; set; }
    public required FindingStatus Status { get; set; }
    public required FindingSeverity Severity { get; set; }
    public required string? Location { get; set; }
    public required string? Snippet { get; set; }
    public required int? StartLine { get; set; }
    public required int? EndLine { get; set; }
    public required string Scanner { get; set; }
    public required ScannerType Type { get; set; }
    public required string? TicketUrl { get; set; }
    public required string? TicketName { get; set; }
}