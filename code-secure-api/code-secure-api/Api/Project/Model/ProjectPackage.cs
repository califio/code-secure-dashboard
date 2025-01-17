using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectPackage
{
    public required Guid Id { get; set; }
    public required string Location { get; set; }
    public required string? Group { get; set; }
    public required string Name { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; }
    public string? FixedVersion { get; set; }
    public required RiskImpact RiskImpact { get; set; }
    public required RiskLevel RiskLevel { get; set; }
}