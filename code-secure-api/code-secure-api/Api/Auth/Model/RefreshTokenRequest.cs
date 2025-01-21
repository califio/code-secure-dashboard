using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Auth.Model;

public record RefreshTokenRequest
{
    [Required] public required string RefreshToken { get; set; }
}