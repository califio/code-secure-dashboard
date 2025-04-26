using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Auth.Model;

public record LogoutRequest
{
    [Required] 
    public required string Token { get; set; }
}
