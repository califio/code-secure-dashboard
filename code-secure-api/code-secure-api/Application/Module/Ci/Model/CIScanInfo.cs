namespace CodeSecure.Application.Module.Ci.Model;

public record CiScanInfo
{
    public required Guid ScanId { get; set; }
    public required string ScanUrl { get; set; }
}