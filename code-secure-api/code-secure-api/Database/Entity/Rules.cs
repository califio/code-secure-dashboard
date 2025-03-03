using CodeSecure.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(Id), nameof(ScannerId))]
public sealed class Rules
{
    public required string Id { get; init; }
    public required RuleStatus Status { get; set; }
    public required RuleConfidence Confidence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required Guid ScannerId { get; set; }
    public Scanners? Scanner { get; set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Rules rule)
        {
            return Id == rule.Id && ScannerId == rule.ScannerId;
        }
        return false;
    }
}