namespace CodeSecure.Api.Dependency.Model;

public record PackageInfo
{
    public required Guid Id { get; set; }
    public required string Group { get; set; }
    public required string Name { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; }
    public required int DependencyCount { get; set; }
    public required bool IsVulnerable { get; set; }
}