using CodeSecure.Application.Module.Mail;
using CodeSecure.Application.Module.User.Command;
using CodeSecure.Application.Module.User.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User;

public interface IUserService
{
    Task<UserDetail> CreateUserAsync(CreateUserRequest request);
    Task<UserDetail> GetUserByIdAsync(Guid userId);
    Task<Page<UserDetail>> GetUserDetailByFilterAsync(UserFilter filter);
    Task<Page<UserSummary>> GetUserSummaryByFilterAsync(UserFilter filter);
    Task<List<UserSummary>> ListProjectManagerUserAsync();
    Task<UserDetail> UpdateUserAsync(Guid userId, UpdateUserRequest request);
    Task<Result<bool>> SendConfirmEmailAsync(Guid userId);
}

public class UserService(AppDbContext context, JwtUserManager userManager, IMailInviteUser mailInviteUser)
    : IUserService
{
    public async Task<UserDetail> CreateUserAsync(CreateUserRequest request)
    {
        var user = (await new CreateUserCommand(userManager, mailInviteUser)
            .ExecuteAsync(request)).GetResult();
        return await GetUserByIdAsync(user.Id);
    }

    public async Task<UserDetail> GetUserByIdAsync(Guid userId)
    {
        return (await new GetUserByIdCommand(context)
            .ExecuteAsync(userId)).GetResult();
    }

    public async Task<Page<UserDetail>> GetUserDetailByFilterAsync(UserFilter filter)
    {
        return (await new GetUserDetailByFilterCommand(context)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<Page<UserSummary>> GetUserSummaryByFilterAsync(UserFilter filter)
    {
        return (await new GetUserSummaryByFilterCommand(context)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<List<UserSummary>> ListProjectManagerUserAsync()
    {
        return (await new ListProjectManagerUserCommand(context)
            .ExecuteAsync()).GetResult();
    }

    public async Task<UserDetail> UpdateUserAsync(Guid userId, UpdateUserRequest request)
    {
        var user = (await new UpdateUserCommand(context, userManager)
            .ExecuteAsync(userId, request)).GetResult();
        return await GetUserByIdAsync(user.Id);
    }

    public async Task<Result<bool>> SendConfirmEmailAsync(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Fail("This account was confirmed");
        }

        _ = mailInviteUser.SendAsync(user.Email!, new MailInviteUserModel
        {
            Username = user.UserName!,
            Token = userManager.GenerateEmailConfirmationTokenAsync(user).Result
        });
        return true;
    }
}