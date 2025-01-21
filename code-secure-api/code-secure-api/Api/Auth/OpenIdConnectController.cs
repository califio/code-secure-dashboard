using System.Security.Claims;
using CodeSecure.Api.Auth.Service;
using CodeSecure.Exception;
using CodeSecure.Extension;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

[Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
public class OpenIdConnectController(IAuthService authService, ILogger<OpenIdConnectController> logger) : ControllerBase
{
    [HttpGet]
    [Route("/api/login/oidc")]
    [Authorize]
    public async Task<IActionResult> LoginOidc()
    {
        var email = User.FindFirstValue("email") ?? User.FindFirstValue(ClaimTypes.Email);
        if (email == null)
        {
            logger.LogInformation("Not found email claim");
            email = User.FindFirstValue(ClaimTypes.Upn);
            if (email == null)
            {
                logger.LogInformation("Not found upn claim");
            }
        }
        if (email == null)
        {
            throw new BadRequestException(
                "Email and upn claim type not found. Please mapping the email to the email or upn attribute");
        }
        if (!email.IsEmail())
        {
            throw new BadRequestException("Email format invalid");
        }
        var result = await authService.OidcSignInAsync(email);
        var queryParams = "oidc=true";
        if (!string.IsNullOrEmpty(result.Message)) queryParams += $"&message={result.Message}";

        if (result.AuthResponse != null)
            queryParams += $"&accessToken={result.AuthResponse.AccessToken}&refreshToken={result.AuthResponse.RefreshToken}";

        var frontendUrl = $"{Request.FrontendUrl()}/#/auth/login?{queryParams}";
        return Redirect(frontendUrl);
    }
}