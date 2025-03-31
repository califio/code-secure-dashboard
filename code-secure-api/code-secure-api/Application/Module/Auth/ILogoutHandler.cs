using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth;

public record LogoutRequest
{
    [Required] public required string Token { get; set; }
}

public interface ILogoutHandler : IHandler<LogoutRequest, bool>;

public class LogoutHandler(JwtUserManager userManager) : ILogoutHandler
{
    public async Task<Result<bool>> HandleAsync(LogoutRequest request)
    {
        return await userManager.LogoutAsync(request.Token);
    }
}