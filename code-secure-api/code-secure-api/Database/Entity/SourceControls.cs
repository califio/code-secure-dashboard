using CodeSecure.Enum;

namespace CodeSecure.Database.Entity;

public class SourceControls: BaseEntity
{
    public required string Url { get; set; }
    public string NormalizedUrl { get; set; } = string.Empty;
    public required SourceType Type { get; set; }
}