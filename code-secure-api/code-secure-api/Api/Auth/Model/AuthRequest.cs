using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Auth.Model;

public record AuthRequest
{
    [Required] public required string UserName { get; set; }

    [Required] public required string Password { get; set; }
}