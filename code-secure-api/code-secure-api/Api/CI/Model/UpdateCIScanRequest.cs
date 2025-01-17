using CodeSecure.Enum;

namespace CodeSecure.Api.CI.Model;

public class UpdateCiScanRequest
{
    public ScanStatus? Status { get; set; }
    public string? Description { get; set; }
}