using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(ScanId), nameof(ProjectPackageId), IsUnique = true)]
public class ScanProjectPackages: BaseEntity
{
    public required Guid ScanId { get; set; }
    public required Guid ProjectPackageId { get; set; }
    public required PackageStatus Status { get; set; }
    public DateTime? FixedAt { get; set; }
    public string? IgnoredReason { get; set; }
    public Guid? UpdatedById { get; set; }
    public Users? UpdatedBy { get; set; }
    public Scans? Scan { get; set; }
    public ProjectPackages? ProjectPackage { get; set; }
}