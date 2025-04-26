using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(NormalizedName), nameof(Type), IsUnique = true)]
public class Scanners : BaseEntity
{
    public required string Name { get; set; }
    public required ScannerType Type { get; set; }
    public string NormalizedName { get; set; } = string.Empty;
}