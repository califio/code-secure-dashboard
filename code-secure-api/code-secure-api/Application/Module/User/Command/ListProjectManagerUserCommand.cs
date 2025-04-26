using CodeSecure.Application.Module.User.Model;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.User.Command;

public class ListProjectManagerUserCommand(AppDbContext context)
{
    public async Task<Result<List<UserSummary>>> ExecuteAsync()
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