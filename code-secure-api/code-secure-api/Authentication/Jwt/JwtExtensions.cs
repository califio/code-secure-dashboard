using System.Security.Claims;
using CodeSecure.Exception;

namespace CodeSecure.Authentication.Jwt;

public static class JwtExtensions
{
    public static UserClaims UserClaims(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(claim => claim.Type == ClaimTypes.Id)?.Value;
        if (userId == null) throw new UnauthorizedException();
        var email = principal.FindFirst(ClaimTypes.Email)?.Value ??
                    principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        return new UserClaims
        {
            Id = Guid.Parse(userId),
            UserName = principal.FindFirst(ClaimTypes.UserName)?.Value!,
            Email = email ?? "",
            Claims = principal.Claims
        };
    }
}