using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record CreateProjectMemberRequest
{
    [Required] public required Guid UserId { get; init; }

    [Required] public required ProjectRole Role { get; init; }
}