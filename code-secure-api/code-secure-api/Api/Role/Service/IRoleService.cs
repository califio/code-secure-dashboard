using CodeSecure.Api.Role.Model;

namespace CodeSecure.Api.Role.Service;

public interface IRoleService
{
    public Task<List<RoleSummary>> GetRolesAsync();
}