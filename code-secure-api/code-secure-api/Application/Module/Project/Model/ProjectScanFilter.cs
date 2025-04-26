using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public sealed record ProjectScanFilter : QueryFilter
{
    public string? Scanner { get; set; }
    public ScannerType? Type { get; set; }
    public ScanStatus? Status { get; set; }
}