using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Token.Model;

public record CreateTokenRequest
{
    [Required] 
    public required string Name { get; set; }
}