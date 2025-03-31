using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeSecure.Authentication;

public class PermissionAttribute : TypeFilterAttribute
{
    public PermissionAttribute(string type, string action) : base(typeof(PermissionFilter))
    {
        Arguments = [type, action];
    }
}

public class PermissionFilter(string permissionType, string action) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == permissionType && c.Value == action);
        if (!hasClaim) context.Result = new ForbidResult();
    }
}