using CodeSecure.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[Index(nameof(PkgId), IsUnique = true)]
public class Packages : BaseEntity
{
    public required string PkgId { get; set; }
    public string? Group { get; set; }
    public required string Name { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; } = "Unknown";
    public string? License { get; set; }
    public required RiskImpact RiskImpact { get; set; }
    public required RiskLevel RiskLevel { get; set; }
    public string? FixedVersion { get; set; }
}