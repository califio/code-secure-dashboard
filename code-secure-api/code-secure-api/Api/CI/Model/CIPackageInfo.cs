namespace CodeSecure.Api.CI.Model;

public record CiPackageInfo
{
    public required string PkgId { get; set; }
    public string? Group { get; set; }
    public required string Name { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; }
    public required string Location { get; set; }
    public required string? License { get; set; }
    public required List<VulnerabilityInfo>? Vulnerabilities { get; set; }
}