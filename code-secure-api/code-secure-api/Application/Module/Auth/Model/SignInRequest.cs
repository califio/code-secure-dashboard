using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Auth.Model;

public record SignInRequest
{
    [Required] public required string UserName { get; set; }

    [Required] public required string Password { get; set; }
}