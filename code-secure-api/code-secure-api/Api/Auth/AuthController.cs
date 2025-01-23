using CodeSecure.Api.Auth.Model;
using CodeSecure.Api.Auth.Service;
using CodeSecure.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : Controller
{
    [HttpGet]
    [Route("/api/server")]
    public string Server()
    {
        return Request.FrontendUrl();
    }

    [HttpPost]
    [Route("/api/login")]
    public async Task<AuthResponse> Login(AuthRequest request)
    {
        return await authService.PasswordSignInAsync(request);
    }

    [HttpPost]
    [Route("/api/refresh-token")]
    public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        return await authService.RefreshTokenAsync(request);
    }

    [HttpPost]
    [Route("/api/logout")]
    public async Task<bool> Logout(LogoutRequest request)
    {
        return await authService.LogoutAsync(request.Token);
    }

    [HttpPost]
    [Route("/api/forgot-password")]
    public async Task ForgotPassword(ForgotPasswordRequest request)
    {
        await authService.ForgotPasswordAsync(request);
    }

    [HttpPost]
    [Route("/api/reset-password")]
    public async Task ResetPassword(ResetPasswordRequest request)
    {
        await authService.ResetPasswordAsync(request);
    }

    [HttpPost]
    [Route("/api/confirm-email")]
    public async Task<ConfirmEmailResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        return await authService.ConfirmEmailAsync(request);
    }
}