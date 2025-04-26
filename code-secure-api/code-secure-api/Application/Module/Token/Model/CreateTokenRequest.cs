using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Token.Model;

public record CreateTokenRequest
{
    [Required] 
    public required string Name { get; set; }
}