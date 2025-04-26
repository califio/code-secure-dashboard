using CodeSecure.Authentication.Jwt;

namespace CodeSecure.Application;

public class ContextCommand(AppDbContext context)
{
    protected readonly AppDbContext Context = context;
}

public class UserContextCommand(AppDbContext context, JwtUserClaims currentUser) : ContextCommand(context)
{
    protected readonly JwtUserClaims CurrentUser = currentUser;
}