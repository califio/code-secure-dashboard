using CodeSecure.Application.Module.Setting;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Application.Module.Auth;

public interface IPasswordSignInHandler : IHandler<SignInRequest, SignInResponse>;

public class PasswordSignInHandler(
    JwtUserManager userManager,
    IAuthSetting authSetting,
    SignInManager<Users> signInManager) : IPasswordSignInHandler
{
    public async Task<Result<SignInResponse>> HandleAsync(SignInRequest request)
    {
        if ((await authSetting.GetAuthSettingAsync()).DisablePasswordLogon)
        {
            return Result.Fail("The admin disabled password logon");
        }

        var user = await userManager.FindByNameAsync(request.UserName) ??
                   await userManager.FindByEmailAsync(request.UserName);
        if (user == null)
        {
            return Result.Fail("Invalid username or password");
        }

        if (user.Status != UserStatus.Active)
        {
            return Result.Fail("The user was disabled");
        }

        var result = await signInManager.PasswordSignInAsync(user, request.Password, true, true);
        if (result.Succeeded)
        {
            return new SignInResponse
            {
                AccessToken = userManager.GenerateAccessToken(user),
                RefreshToken = userManager.GenerateRefreshToken(user)
            };
        }

        if (result.RequiresTwoFactor)
        {
            return SignInResponse.NeedTwoFactor;
        }

        if (result.IsNotAllowed)
        {
            return SignInResponse.NeedConfirmEmail;
        }

        return Result.Fail(result.IsLockedOut ? "The account was locked out" : "Invalid username or password");
    }
}