using CodeSecure.Api.Auth.Model;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Extension;
using CodeSecure.Manager.Notification;
using CodeSecure.Manager.Notification.Model;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Api.Auth.Service;

public class DefaultAuthService(
    IHttpContextAccessor contextAccessor,
    SignInManager<Users> signInManager,
    JwtUserManager userManager,
    IAppSettingManager appSettingManager,
    IMailSender mailSender
) : IAuthService
{
    public async Task<AuthResponse> PasswordSignInAsync(AuthRequest request)
    {
        var authSetting = (await appSettingManager.AppSettingAsync()).AuthSetting;
        if (authSetting is { DisablePasswordLogon: true })
            throw new BadRequestException("the admin disabled password logon");

        var user = await userManager.FindByNameAsync(request.UserName) ??
                   await userManager.FindByEmailAsync(request.UserName);
        if (user == null) throw new BadRequestException("username or password incorrect");

        if (user.Status != UserStatus.Active) throw new BadRequestException("the user was disabled");

        var result = await signInManager.PasswordSignInAsync(user, request.Password, true, true);
        if (result.Succeeded)
            return new AuthResponse
            {
                AccessToken = userManager.GenerateAccessToken(user),
                RefreshToken = userManager.GenerateRefreshToken(user),
                RequireTwoFactor = false
            };

        if (result.RequiresTwoFactor)
            return new AuthResponse
            {
                AccessToken = "",
                RefreshToken = "",
                RequireTwoFactor = true
            };

        if (result.IsNotAllowed)
            return new AuthResponse
            {
                AccessToken = "",
                RefreshToken = "",
                RequireConfirmEmail = true
            };

        if (result.IsLockedOut) throw new BadRequestException("the account is locked out");

        throw new BadRequestException("username or password incorrect");
    }

    public async Task<OidcSignInResult> OidcSignInAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        string? message = null;
        if (user == null)
        {
            var username = email.Split('@')[0];
            var status = Application.Setting.AuthSetting.AllowRegister ? UserStatus.Active : UserStatus.Disabled;
            user = new Users
            {
                Id = Guid.NewGuid(),
                UserName = username,
                Email = email,
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                FullName = username,
                Status = status,
                Avatar = null,
                IsDefault = false,
                CreatedAt = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(user, PasswordGenerator.GeneratePassword(32));
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
                if (!Application.Setting.AuthSetting.AllowRegister)
                {
                    message = $"Registration successful. Contact admin to enable your account: {email}";
                }
            }
            else
            {
                using var enumerator = result.Errors.GetEnumerator();
                message = $"Registration fail: {enumerator.Current.Description}";
            }
        }
        else
        {
            if (user.Status != UserStatus.Active)
            {
                message = "Your account was disabled";
            }
        }

        if (user.Status != UserStatus.Active)
        {
            return new OidcSignInResult
            {
                AuthResponse = null,
                Message = message
            };
        }

        return new OidcSignInResult
        {
            AuthResponse = new AuthResponse
            {
                AccessToken = userManager.GenerateAccessToken(user),
                RefreshToken = userManager.GenerateRefreshToken(user),
                RequireTwoFactor = false
            },
            Message = null
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var result = await userManager.RefreshTokenAsync(request.RefreshToken);
        if (result.IsUnauthorized) throw new UnauthorizedException();

        return new AuthResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            RequireTwoFactor = false
        };
    }

    public async Task<bool> LogoutAsync(string token)
    {
        return await userManager.LogoutAsync(token);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username)
                   ?? await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var frontendUrl = contextAccessor.FrontendUrl();
        var resetPasswordUrl =
            $"{frontendUrl}/#/auth/reset-password?token={token.UrlEncode()}&username={user.UserName}";
        mailSender.SendResetPassword([user.Email!], new ResetPasswordModel
        {
            Username = user.UserName!,
            ResetPasswordUrl = resetPasswordUrl
        });
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username)
                   ?? await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            throw new BadRequestException("Username invalid");
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Errors.First().Description);
        }
    }

    public async Task<ConfirmEmailResult> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username)
                   ?? await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return ConfirmEmailResult.Failed("Username invalid");
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            throw new BadRequestException("The account has been confirmed");
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return ConfirmEmailResult.Failed(result.Errors.First().Description);
        }

        return ConfirmEmailResult.Success;
    }
}