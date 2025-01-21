using CodeSecure.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[Index(nameof(NormalizedName), IsUnique = true)]
public class Scanners : BaseEntity
{
    public required string Name { get; set; }
    public required ScannerType Type { get; set; }
    public string NormalizedName { get; set; } = string.Empty;
}