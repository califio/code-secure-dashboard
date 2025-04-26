using CodeSecure.Application.Module.Role.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Role;

public interface IRoleService
{
    Task<List<RoleSummary>> ListRoleAsync();
}

public class RoleService(AppDbContext context) : IRoleService
{
    public async Task<List<RoleSummary>> ListRoleAsync()
    {
        return await context.Roles.Select(record => new RoleSummary
        {
            IsDefault = record.IsDefault,
            Name = record.Name
        }).ToListAsync();
    }
}