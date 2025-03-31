using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Role;

public interface IListRoleHandler : IOutputHandler<List<RoleSummary>>;
public class ListRoleHandler(AppDbContext context): IListRoleHandler
{
    public async Task<Result<List<RoleSummary>>> HandleAsync()
    {
        return await context.Roles.Select(record => new RoleSummary
        {
            IsDefault = record.IsDefault,
            Name = record.Name
        }).ToListAsync();
    }
}