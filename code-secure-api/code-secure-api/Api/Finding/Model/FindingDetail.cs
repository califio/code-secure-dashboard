using CodeSecure.Database.Entity;
using CodeSecure.Database.Metadata;
using CodeSecure.Enum;

namespace CodeSecure.Api.Finding.Model;

public record FindingDetail
{
    public required Guid Id { get; set; }
    public required string Identity { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Recommendation { get; set; }
    public required FindingLocation Location { get; set; }
    public FindingMetadata? Metadata { get; set; }
    public required FindingStatus Status { get; set; }
    public required FindingSeverity Severity { get; set; }
    public required FindingProject Project { get; set; }
    public required string Scanner { get; set; }
    public required ScannerType Type { get; set; }
    public required DateTime? FixDeadline { get; set; }

    public required List<FindingScan> Scans { get; set; }
    public required Tickets? Ticket { get; set; }
}