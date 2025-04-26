using CodeSecure.Application.Module.Auth.Model;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth.Command;

public class RefreshTokenCommand(JwtUserManager userManager)
{
    public async Task<Result<SignInResponse>> ExecuteAsync(RefreshTokenRequest request)
    {
        var result = await userManager.RefreshTokenAsync(request.RefreshToken);
        if (result.IsUnauthorized)
        {
            Result.Fail("Unauthorized");
        }

        return new SignInResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
        };
    }
}