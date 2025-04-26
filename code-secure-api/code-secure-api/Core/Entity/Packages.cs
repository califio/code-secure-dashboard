using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(PkgId), IsUnique = true)]
public class Packages : BaseEntity
{
    public required string PkgId { get; init; }
    public string? Group { get; set; }
    public required string Name { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; } = "Unknown";
    public string? License { get; set; }
    public required RiskImpact RiskImpact { get; set; }
    public required RiskLevel RiskLevel { get; set; }
    public string? FixedVersion { get; set; }

    public string FullName()
    {
        if (string.IsNullOrEmpty(Group))
        {
            return Name;
        }

        return $"{Group}.{Name}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Packages pkg)
        {
            return PkgId == pkg.PkgId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return PkgId.GetHashCode();
    }
}