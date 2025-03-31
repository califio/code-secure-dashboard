using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[PrimaryKey(nameof(ContainerId), nameof(PackageId))]

public class ContainerPackages
{
    public required Guid ContainerId { get; set; }
    public required Guid PackageId { get; set; }
    public Containers? Container { get; set; }
    public Packages? Package { get; set; }
}