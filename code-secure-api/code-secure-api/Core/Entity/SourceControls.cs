using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(NormalizedUrl), nameof(Type), IsUnique = true)]
public class SourceControls: BaseEntity
{
    public required string Url { get; set; }
    public string NormalizedUrl { get; set; } = string.Empty;
    public required SourceType Type { get; set; }
}