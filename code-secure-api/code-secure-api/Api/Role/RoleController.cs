using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Role;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Role;

public class RoleController(IListRoleHandler listRoleHandler) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Role, PermissionAction.Read)]
    public async Task<List<RoleSummary>> GetRoles()
    {
        var result = await listRoleHandler.HandleAsync();
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.ListErrors());
    }
}