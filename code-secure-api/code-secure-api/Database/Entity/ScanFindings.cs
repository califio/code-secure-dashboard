using CodeSecure.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ScanId), nameof(FindingId))]
public class ScanFindings
{
    public required Guid ScanId { get; set; }
    public required Guid FindingId { get; set; }
    public required FindingStatus Status { get; set; }
    public required string CommitHash { get; set; }
    public Scans? Scan { get; set; }
    public Findings? Finding { get; set; }
}