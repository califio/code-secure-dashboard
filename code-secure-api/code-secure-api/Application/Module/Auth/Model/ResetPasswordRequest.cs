using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Auth.Model;

public class ResetPasswordRequest
{
    [Required]
    public required string Token { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}