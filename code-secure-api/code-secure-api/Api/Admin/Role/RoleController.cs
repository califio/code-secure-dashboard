using CodeSecure.Api.Admin.Role.Model;
using CodeSecure.Api.Admin.Role.Service;
using CodeSecure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Admin.Role;

public class RoleController(IRoleService roleService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Role, PermissionAction.Read)]
    public async Task<List<RoleSummary>> GetRoles()
    {
        return await roleService.GetRolesAsync();
    }
}