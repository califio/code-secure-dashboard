using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.User.Model;

public record CreateUserRequest
{
    [EmailAddress] 
    public required string Email { get; set; }
    public bool Verified { get; set; }
    [Required] 
    public required string Role { get; set; }
}