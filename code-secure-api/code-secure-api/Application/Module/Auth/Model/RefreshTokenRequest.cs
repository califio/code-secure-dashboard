using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Auth.Model;

public record RefreshTokenRequest
{
    [Required] public required string RefreshToken { get; set; }
}