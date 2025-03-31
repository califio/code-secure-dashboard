using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleConfidence
{
    High,
    Medium,
    Low,
    Unknown
}