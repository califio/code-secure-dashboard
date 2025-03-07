namespace CodeSecure.Api.CI.Model;

public record ScanDependencyResult
{
    public required List<CiPackageInfo> Packages { get; set; }
    public required bool IsBlock { get; set; }
    public required string Scanner { get; set; }
    public required string ScanUrl { get; set; }
}