using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FindingStatus
{
    Open,
    Confirmed,
    AcceptedRisk,
    Fixed,
    Incorrect
}