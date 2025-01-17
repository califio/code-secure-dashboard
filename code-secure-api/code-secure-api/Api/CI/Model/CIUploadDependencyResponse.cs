namespace CodeSecure.Api.CI.Model;

public record CiUploadDependencyResponse
{
    public required IEnumerable<CiPackageInfo> Packages { get; set; }
    public required bool IsBlock { get; set; }
}