using CodeSecure.Enum;

namespace CodeSecure.Manager.Integration.Model;

public record FindingModel
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public required FindingSeverity Severity { get; set; }
}