using CodeSecure.Authentication.Jwt;

namespace CodeSecure.Application;

public interface IAuthorize<in T>
{
    bool Authorize(T resource, JwtUserClaims user, string permission);
}

