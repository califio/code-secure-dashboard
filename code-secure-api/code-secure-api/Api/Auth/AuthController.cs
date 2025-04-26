using CodeSecure.Application.Module.Auth;
using CodeSecure.Application.Module.Auth.Command;
using CodeSecure.Application.Module.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[AllowAnonymous]
public class AuthController(
    IAuthService authService
) : Controller
{
    [HttpGet]
    [Route("/api/auth-config")]
    public async Task<AuthConfig> GetAuthConfig()
    {
        return await authService.GetAuthConfigAsync();
    }

    [HttpPost]
    [Route("/api/login")]
    public Task<SignInResponse> Login(SignInRequest request)
    {
        return authService.PasswordSignInAsync(request);
    }

    [HttpPost]
    [Route("/api/refresh-token")]
    public Task<SignInResponse> RefreshToken(RefreshTokenRequest request)
    {
        return authService.RefreshTokenAsync(request);
    }

    [HttpPost]
    [Route("/api/logout")]
    public Task<bool> Logout(LogoutRequest request)
    {
        return authService.LogoutAsync(request);
    }

    [HttpPost]
    [Route("/api/forgot-password")]
    public async Task<bool> ForgotPassword(ForgotPasswordRequest request)
    {
        return await authService.ForgotPasswordAsync(request);
    }

    [HttpPost]
    [Route("/api/reset-password")]
    public Task<bool> ResetPassword(ResetPasswordRequest request)
    {
        return authService.ResetPasswordAsync(request);
    }

    [HttpPost]
    [Route("/api/confirm-email")]
    public async Task<bool> ConfirmEmail(ConfirmEmailRequest request)
    {
        return await authService.ConfirmEmailAsync(request);
    }
}