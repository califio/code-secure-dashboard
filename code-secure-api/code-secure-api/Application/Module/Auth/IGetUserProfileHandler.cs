using CodeSecure.Api;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Auth;

public interface IGetUserProfileHandler: IHandler<Guid, UserProfile>;
public class GetUserProfileHandler(AppDbContext context) : BaseController, IGetUserProfileHandler
{
    public async Task<Result<UserProfile>> HandleAsync(Guid request)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request);
        if (user == null) return Result.Fail("User not found");
        return new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName!,
            FullName = user.FullName,
            Email = user.Email,
            Avatar = user.Avatar
        };
    }
}