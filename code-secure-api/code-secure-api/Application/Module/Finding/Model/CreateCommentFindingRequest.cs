using System.Text.Json.Serialization;
using CodeSecure.Authentication.Jwt;

namespace CodeSecure.Application.Module.Finding.Model;

public record CreateCommentFindingRequest
{
    public Guid FindingId { get; set; }
    public required string Comment { get; set; }
    [JsonIgnore] 
    public required JwtUserClaims CurrentUser { get; set; }
}