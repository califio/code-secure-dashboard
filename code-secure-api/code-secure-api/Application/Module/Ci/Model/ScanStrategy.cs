using System.Text.Json.Serialization;

namespace CodeSecure.Application.Module.Ci.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScanStrategy
{
    AllFiles = 1,
    ChangedFileOnly = 2
}