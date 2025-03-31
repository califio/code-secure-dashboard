using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[PrimaryKey(nameof(PackageId), nameof(DependencyId))]
public class PackageDependencies
{
    public required Guid PackageId { get; set; }
    public required Guid DependencyId { get; set; }

    public Packages? Package { get; set; }
    public Packages? Dependency { get; set; }
}