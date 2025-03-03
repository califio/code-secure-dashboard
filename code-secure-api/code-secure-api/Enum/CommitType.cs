using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommitType
{
    CommitBranch,
    CommitTag,
    MergeRequest,
}