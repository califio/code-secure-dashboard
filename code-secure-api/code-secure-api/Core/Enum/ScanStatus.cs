using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScanStatus
{
    Queue,
    Running,
    Completed,
    Error
}