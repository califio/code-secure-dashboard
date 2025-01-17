using System.Security.Claims;

namespace CodeSecure.Authentication.Jwt;

public class UserClaims
{
    public required Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required IEnumerable<Claim> Claims { get; set; }

    public bool HasClaim(string claimType, string claimValue)
    {
        return Claims.Any(claim => claim.Type == claimType && claim.Value == claimValue);
    }
}