using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User;

public interface IListProjectManagerUserHandler : IHandler<bool, List<UserSummary>>;
public class ListProjectManagerUserHandler(AppDbContext context): IListProjectManagerUserHandler
{
    public async Task<Result<List<UserSummary>>> HandleAsync(bool request)
    {
        var users = await context.ProjectUsers
            .Include(record => record.User)
            .Where(record => record.Role == ProjectRole.Manager)
            .Select(record => record.User!).Distinct().ToListAsync();
        return users.Select(user => new UserSummary
        {
            Id = user.Id,
            UserName = user.UserName!,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Email = user.Email
        }).ToList();
    }
}