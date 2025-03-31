using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

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