using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleConfidence
{
    High,
    Medium,
    Low,
    Unknown
}