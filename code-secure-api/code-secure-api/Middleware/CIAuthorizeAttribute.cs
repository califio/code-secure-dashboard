using CodeSecure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CiAuthorizeAttribute(AppDbContext dbContext) : Attribute, IAsyncAuthorizationFilter
{
    private const string TokenHeader = "CI-TOKEN";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(TokenHeader, out var token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var isValid = await dbContext.CiTokens.AnyAsync(record => record.Value == token);
        if (!isValid) context.Result = new UnauthorizedResult();
    }
}