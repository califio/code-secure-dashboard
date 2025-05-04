using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(Identity), nameof(ProjectId), IsUnique = true)]
public sealed class Findings : BaseEntity
{
    public required string Identity { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Category { get; init; }
    public string? Recommendation { get; set; }
    public required FindingStatus Status { get; set; }
    public required FindingSeverity Severity { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? FixedAt { get; set; }
    public DateTime? FixDeadline { get; set; }
    public string? RuleId { get; init; }
    public string? Location { get; set; }
    public string? Snippet { get; set; }
    public int? StartLine { get; set; }
    public int? EndLine { get; set; }
    public int? StartColumn { get; set; }
    public int? EndColumn { get; set; }
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