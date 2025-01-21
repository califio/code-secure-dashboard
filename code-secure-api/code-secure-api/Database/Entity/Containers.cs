using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[Index(nameof(NormalizedName), IsUnique = true)]
public class Containers: BaseEntity
{
    public required string Name { get; set; }
    public string NormalizedName { get; set; } = string.Empty;
    public string? OsName { get; set; }
    public string? OsVersion { get; set; }
}