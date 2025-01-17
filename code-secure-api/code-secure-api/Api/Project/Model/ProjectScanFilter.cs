using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public sealed record ProjectScanFilter : QueryFilter
{
    public string? Scanner { get; set; }
    public ScannerType? Type { get; set; }
    public ScanStatus? Status { get; set; }
}