using CodeSecure.Database.Entity;

namespace CodeSecure.Api.Project.Model;

public record ProjectPackageDetail
{
    public required Packages Info { get; set; }
    public required string Location { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
    public required List<Packages> Dependencies { get; set; }
    public required List<BranchStatusPackage> BranchStatus { get; set; }
}