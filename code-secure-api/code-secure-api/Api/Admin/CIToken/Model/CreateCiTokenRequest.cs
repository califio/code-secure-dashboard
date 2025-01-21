using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Admin.CIToken.Model;

public record CreateCiTokenRequest
{
    [Required] public required string Name { get; set; }
}