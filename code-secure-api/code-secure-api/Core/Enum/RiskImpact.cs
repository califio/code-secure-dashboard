using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskImpact
{
    None,
    Indirect,
    Direct
}