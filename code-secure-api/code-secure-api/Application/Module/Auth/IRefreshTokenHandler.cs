using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth;

public record RefreshTokenRequest
{
    [Required] public required string RefreshToken { get; set; }
}
public interface IRefreshTokenHandler : IHandler<RefreshTokenRequest, SignInResponse>;
public class RefreshTokenHandler(JwtUserManager userManager) : IRefreshTokenHandler
{
    public async Task<Result<SignInResponse>> HandleAsync(RefreshTokenRequest request)
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