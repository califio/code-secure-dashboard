using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Auth.Model;

public class ConfirmEmailRequest
{
    [Required]
    public required string Token { get; set; }
    [Required]
    public required string Username { get; set; }
}