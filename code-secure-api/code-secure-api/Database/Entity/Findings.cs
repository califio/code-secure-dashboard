using CodeSecure.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[Index(nameof(Identity), nameof(ProjectId), IsUnique = true)]
public sealed class Findings : BaseEntity
{
    public required string Identity { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? Category { get; init; }
    public string? Recommendation { get; set; }
    public required FindingStatus Status { get; set; }
    public required FindingSeverity Severity { get; set; }
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
    
    public Guid? TicketId { get; set; }
    public Tickets? Ticket { get; set; }

    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }

    public bool Equals(Findings? other)
    {
        return Identity.Equals(other?.Identity);
    }
}