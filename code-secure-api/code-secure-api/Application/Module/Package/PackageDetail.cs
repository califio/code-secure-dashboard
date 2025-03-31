using CodeSecure.Core.Entity;

namespace CodeSecure.Application.Module.Package;

public record PackageDetail
{
    public required Packages Info { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
    public required List<Packages> Dependencies { get; set; }
}