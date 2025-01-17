namespace CodeSecure.Api.CI.Model;

public record CiPackageInfo
{
    public required CiPackage Package { get; set; }
    public required List<CiVulnerability> Vulnerabilities { get; set; }
}