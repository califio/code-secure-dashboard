using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.User.Model;

public record CreateUserRequest
{
    [EmailAddress] public required string Email { get; set; }

    [Required] public required string Role { get; set; }
}