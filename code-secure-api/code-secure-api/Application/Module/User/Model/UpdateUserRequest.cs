using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.User.Model;

public class UpdateUserRequest
{
    [StringLength(64)] 
    public string? FullName { get; set; }
    [StringLength(64)] 
    [EmailAddress] 
    public string? Email { get; set; }
    public bool? Verified { get; set; }
    public UserStatus? Status { get; set; }
    public string? Role { get; set; }
}