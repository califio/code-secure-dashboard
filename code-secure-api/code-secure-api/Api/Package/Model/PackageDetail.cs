using CodeSecure.Database.Entity;

namespace CodeSecure.Api.Package.Model;

public record PackageDetail
{
    public required Packages Info { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
    public required List<Packages> Dependencies { get; set; }
}