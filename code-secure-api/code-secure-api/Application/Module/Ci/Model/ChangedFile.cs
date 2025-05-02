using System.Text.Json.Serialization;

namespace CodeSecure.Application.Module.Ci.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChangedFileStatus
{
    Add = 1,
    Modify = 2,
    Delete = 3
}
public record ChangedFile
{
    public string? From { get; set; }
    public string? To { get; set; }
    public required ChangedFileStatus Status { get; set; }
}