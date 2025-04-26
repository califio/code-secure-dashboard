using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Auth.Command;
using CodeSecure.Application.Module.Auth.Model;
using CodeSecure.Application.Module.Mail;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Application.Module.Auth;

public interface IAuthService
{
    Task<AuthConfig> GetAuthConfigAsync();
    Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<UserProfile> GetUserProfileAsync(Guid userId);
    Task<OpenIdConnectSignInResponse> OpenIdConnectSignInAsync(string email);
    Task<SignInResponse> PasswordSignInAsync(SignInRequest request);
    Task<SignInResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
    Task<bool> LogoutAsync(LogoutRequest request);
}

public class AuthService(
    AppDbContext context,
    JwtUserManager userManager,
    SignInManager<Users> signInManager,
    IMailResetPassword mailResetPassword
) : IAuthService
{
    public async Task<AuthConfig> GetAuthConfigAsync()
    {
        var setting = await context.GetAuthSettingAsync();
        return new AuthConfig
        {
            DisablePasswordLogon = setting.DisablePasswordLogon,
            OpenIdConnectEnable = setting.OpenIdConnectSetting is { Enable: true },
            OpenIdConnectProvider = setting.OpenIdConnectSetting.DisplayName
        };
    }

    public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        return (await new ConfirmEmailCommand(userManager)
                .ExecuteAsync(request)).GetResult();
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return (await new ForgotPasswordCommand(userManager, mailResetPassword)
                .ExecuteAsync(request)).GetResult();
    }

    public async Task<UserProfile> GetUserProfileAsync(Guid userId)
    {
        return (await new GetUserProfileCommand(context)
                .ExecuteAsync(userId)).GetResult();
    }

    public async Task<OpenIdConnectSignInResponse> OpenIdConnectSignInAsync(string email)
    {
        return (await new OpenIdConnectSignInCommand(context, userManager)
            .ExecuteAsync(email)).GetResult();
    }

    public async Task<SignInResponse> PasswordSignInAsync(SignInRequest request)
    {
        return (await new PasswordSignInCommand(context, userManager, signInManager)
            .ExecuteAsync(request)).GetResult();
    }

    public async Task<SignInResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var result = await new RefreshTokenCommand(userManager).ExecuteAsync(request);
        if (result.IsFailed)
        {
            throw new UnauthorizedException();
        }
        return result.Value;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return (await new ResetPasswordCommand(userManager)
            .ExecuteAsync(request)).GetResult();
    }

    public Task<bool> LogoutAsync(LogoutRequest request)
    {
        return userManager.LogoutAsync(request.Token);
    }
}