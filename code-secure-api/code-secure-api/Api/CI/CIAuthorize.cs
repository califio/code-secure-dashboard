using CodeSecure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeSecure.Api.CI;

public interface ICiAuthorize
{
    bool IsAuthorize(string token);
}

public class CiAuthorize(AppDbContext context) : ICiAuthorize
{
    public bool IsAuthorize(string token)
    {
        return context.CiTokens.Any(record => record.Value == token);
    }
}

public class CiAuthorizeAttribute() : TypeFilterAttribute(typeof(CiAuthorizeFilter));

public class CiAuthorizeFilter : IAuthorizationFilter
{
    private const string TokenHeader = "CI-TOKEN";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(TokenHeader, out var token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var ciAuthorize = context.HttpContext.RequestServices.GetRequiredService<ICiAuthorize>();
        var isValid = ciAuthorize.IsAuthorize(token!);
        if (!isValid) context.Result = new UnauthorizedResult();
    }
}