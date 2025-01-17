using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(PackageId), nameof(VulnerabilityId))]
public class PackageVulnerabilities
{
    public required Guid PackageId { get; set; }
    public required Guid VulnerabilityId { get; set; }

    public Packages? Package { get; set; }
    public Vulnerabilities? Vulnerability { get; set; }
}