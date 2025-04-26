using System.Text.Json.Serialization;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record UpdateProjectMemberRequest
{
    public required ProjectRole Role { get; set; }
    [JsonIgnore] public Guid UserId { get; set; }
}