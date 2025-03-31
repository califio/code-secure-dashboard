using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth;

public partial class ResetPasswordRequest
{
    [Required]
    public required string Token { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}

public interface IResetPasswordHandler : IHandler<ResetPasswordRequest, bool>;

public class ResetPasswordHandler(JwtUserManager userManager) : IResetPasswordHandler
{
    public async Task<Result<bool>> HandleAsync(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username)
                   ?? await userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            return Result.Fail("Invalid username");
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            return Result.Fail(result.Errors.First().Description);
        }

        return Result.Ok();
    }
}