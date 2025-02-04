using CodeSecure.Api.Role.Model;
using CodeSecure.Database;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Role.Service;

public class DefaultRoleService(AppDbContext context) : IRoleService
{
    public async Task<List<RoleSummary>> GetRolesAsync()
    {
        return await context.Roles.Select(record => new RoleSummary
        {
            IsDefault = record.IsDefault,
            Name = record.Name
        }).ToListAsync();
    }
}