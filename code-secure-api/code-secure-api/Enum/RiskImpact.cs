using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskImpact
{
    None,
    Indirect,
    Direct
}