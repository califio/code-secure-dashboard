using CodeSecure.Authentication.Jwt;
using CodeSecure.Exception;
using CodeSecure.Extension;

namespace CodeSecure.Api;

public abstract class BaseService<TEntity>(IHttpContextAccessor contextAccessor)
{
    private UserClaims? userClaims;

    protected UserClaims CurrentUser()
    {
        var principal = contextAccessor.HttpContext?.User;
        if (principal == null) throw new UnauthorizedException();
        return userClaims ??= principal.UserClaims();
    }

    protected abstract bool HasPermission(TEntity entity, string action);
    protected abstract Task<TEntity> FindByIdAsync(Guid id);

    protected string FrontendUrl()
    {
        return contextAccessor.HttpContext != null ? contextAccessor.HttpContext.Request.FrontendUrl() : string.Empty;
    }
}