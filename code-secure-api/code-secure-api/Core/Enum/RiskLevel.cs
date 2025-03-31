using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskLevel
{
    None,
    Low,
    Medium,
    High,
    Critical
}