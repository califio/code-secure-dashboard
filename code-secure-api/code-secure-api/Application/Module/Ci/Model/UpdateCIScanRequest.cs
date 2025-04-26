using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Ci.Model;

public class UpdateCiScanRequest
{
    public ScanStatus? Status { get; set; }
    public string? Description { get; set; }
}