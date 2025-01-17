using CodeSecure.Api.Admin.Role.Model;

namespace CodeSecure.Api.Admin.Role.Service;

public interface IRoleService
{
    public Task<List<RoleSummary>> GetRolesAsync();
}