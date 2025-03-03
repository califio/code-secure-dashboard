using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FindingActivityType
{
    Open,
    Fixed,
    Reopen,
    Comment,
    ChangeStatus,
    ChangeDeadline,
    ChangeSeverity,
}