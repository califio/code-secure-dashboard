using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Auth.Model;

public class ForgotPasswordRequest
{
    [Required]
    public required string Username { get; set; }
}