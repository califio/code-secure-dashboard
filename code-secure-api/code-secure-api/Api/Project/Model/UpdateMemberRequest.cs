using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record UpdateMemberRequest
{
    public required ProjectRole Role { get; set; }
}