using CodeSecure.Application.Module.Auth.Model;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.Auth.Command;

public class OpenIdConnectSignInCommand(
    AppDbContext context,
    JwtUserManager userManager)
{
    public async Task<Result<OpenIdConnectSignInResponse>> ExecuteAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        string? message = null;
        if (user == null)
        {
            var username = email.Split('@')[0];
            var allowRegister = (await context.GetAuthSettingAsync()).AllowRegister;
            var status = allowRegister ? UserStatus.Active : UserStatus.Disabled;
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
                if (!allowRegister)
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
            return new OpenIdConnectSignInResponse { Message = message };
        }

        return new OpenIdConnectSignInResponse
        {
            AuthResponse = new SignInResponse
            {
                AccessToken = userManager.GenerateAccessToken(user),
                RefreshToken = userManager.GenerateRefreshToken(user),
            }
        };
    }
}