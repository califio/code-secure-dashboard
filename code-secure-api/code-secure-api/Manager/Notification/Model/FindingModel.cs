using CodeSecure.Enum;

namespace CodeSecure.Manager.Notification.Model;

public record FindingModel
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public required FindingSeverity Severity { get; set; }
}