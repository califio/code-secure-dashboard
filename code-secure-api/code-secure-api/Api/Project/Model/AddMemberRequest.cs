using System.ComponentModel.DataAnnotations;
using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record AddMemberRequest
{
    [Required] public required Guid UserId { get; set; }

    [Required] public required ProjectRole Role { get; set; }
}