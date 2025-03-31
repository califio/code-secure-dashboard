using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.Jwt;
using FluentResults;

namespace CodeSecure.Application.Module.Auth;

public class ConfirmEmailRequest
{
    [Required]
    public required string Token { get; set; }
    [Required]
    public required string Username { get; set; }
}

public class ConfirmEmailResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}

public interface IConfirmEmailHandler : IHandler<ConfirmEmailRequest, ConfirmEmailResponse>;

public class ConfirmEmailHandler(JwtUserManager userManager) : IConfirmEmailHandler
{
    public async Task<Result<ConfirmEmailResponse>> HandleAsync(ConfirmEmailRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Username)
                   ?? await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return new ConfirmEmailResponse { Error = "Username invalid" };
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Fail("The account has been confirmed");
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return new ConfirmEmailResponse { Error = result.Errors.First().Description };
        }

        return new ConfirmEmailResponse { Success = true };
    }
}