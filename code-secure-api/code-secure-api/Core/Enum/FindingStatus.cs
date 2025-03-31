using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FindingStatus
{
    Open,
    Confirmed,
    AcceptedRisk,
    Fixed,
    Incorrect
}