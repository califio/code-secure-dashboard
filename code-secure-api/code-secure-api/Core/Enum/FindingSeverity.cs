using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FindingSeverity
{
    Info,
    Low,
    Medium,
    High,
    Critical
}