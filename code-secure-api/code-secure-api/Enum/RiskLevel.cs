using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskLevel
{
    None,
    Low,
    Medium,
    High,
    Critical
}