using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(Identity), IsUnique = true)]
public class Vulnerabilities : BaseEntity
{
    // cve id
    public required string Identity { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required FindingSeverity Severity { get; set; }
    public required string PkgName { get; set; }
    public required string FixedVersion { get; set; }
    public DateTime? PublishedAt { get; set; }
}