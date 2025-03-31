using System.Security.Claims;
using CodeSecure.Application;
using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Auth;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

[Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
public class OpenIdConnectController(IOpenIdConnectSignInHandler openIdConnectSignInHandler, ILogger<OpenIdConnectController> logger) : ControllerBase
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
        var result = await openIdConnectSignInHandler.HandleAsync(email);
        if (!result.IsSuccess) throw new BadRequestException(result.Errors.Select(error => error.Message));
        var queryParams = "oidc=true";
        if (!string.IsNullOrEmpty(result.Value.Message)) queryParams += $"&message={result.Value.Message}";
        if (result.Value.AuthResponse != null)
            queryParams += $"&accessToken={result.Value.AuthResponse.AccessToken}&refreshToken={result.Value.AuthResponse.RefreshToken}";

        var frontendUrl = $"{Configuration.FrontendUrl}/#/auth/login?{queryParams}";
        return Redirect(frontendUrl);
    }
}