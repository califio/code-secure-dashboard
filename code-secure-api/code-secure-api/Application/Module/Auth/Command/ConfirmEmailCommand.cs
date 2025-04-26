using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth.Command;

public class ConfirmEmailRequest
{
    [Required] public required string Token { get; set; }
    [Required] public required string Username { get; set; }
}

public class ConfirmEmailCommand(JwtUserManager userManager)
{
    public async Task<Result<bool>> ExecuteAsync(ConfirmEmailRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username)
                   ?? await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Result.Fail("Username invalid");
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Fail("The account has been confirmed");
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return Result.Fail(result.Errors.First().Description);
        }

        return true;
    }
}