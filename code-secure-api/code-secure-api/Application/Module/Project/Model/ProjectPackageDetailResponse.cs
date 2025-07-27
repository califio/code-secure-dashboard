using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectPackageDetailResponse
{
    public required Packages Info { get; set; }
    public required string Location { get; set; }
    public required PackageStatus? Status { get; set; }
    public required string? IgnoreReason { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
    public required List<Packages> Dependencies { get; set; }
    public required List<BranchStatusPackage> BranchStatus { get; set; }
    public required Tickets? Ticket { get; set; }
    public required Guid ProjectId { get; set; }
    public required string ProjectName { get; set; }
}