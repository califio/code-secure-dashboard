using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CodeSecure.Application;
using CodeSecure.Core.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CodeSecure.Authentication.Jwt;

public class JwtUserManager(
    AppDbContext context,
    RoleManager<Roles> roleManager,
    IUserStore<Users> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<Users> passwordHasher,
    IEnumerable<IUserValidator<Users>> userValidators,
    IEnumerable<IPasswordValidator<Users>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<JwtUserManager> logger
) : UserManager<Users>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
    errors, services, logger)
{
    private const string RefreshTokenName = "RefreshToken";
    private const int AccessTokenTimeout = 5; // 5m
    private const int RefreshTokenTimeout = 30; //day

    private readonly SigningCredentials accessTokenKey =
        new(Configuration.AccessTokenKey, SecurityAlgorithms.HmacSha256Signature);

    private readonly SigningCredentials refreshTokenKey =
        new(Configuration.RefreshTokenKey, SecurityAlgorithms.HmacSha256Signature);

    private readonly JwtSecurityTokenHandler tokenHandler = new();

    public async Task<RefreshTokenResult> RefreshTokenAsync(string token)
    {
        try
        {
            // validate jwt token first
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = Configuration.RefreshTokenKey,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var securityToken);
            // validate jwt token in db
            var userToken = await context.UserTokens.FirstOrDefaultAsync(userToken => userToken.Value == token);
            if (userToken == null) return RefreshTokenResult.Unauthorized;

            var jwtToken = (JwtSecurityToken)securityToken;
            var email = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email)?.Value;
            var user = await context.Users.FirstOrDefaultAsync(user =>
                user.Id == userToken.UserId && user.Email == email);
            if (user != null)
            {
                var refreshToken = GenerateRefreshToken(user, false);
                userToken.Value = refreshToken;
                userToken.ExpiredAt = DateTime.UtcNow.AddDays(RefreshTokenTimeout);
                context.Update(userToken);
                await context.SaveChangesAsync();
                return new RefreshTokenResult
                {
                    Success = true,
                    IsUnauthorized = false,
                    AccessToken = GenerateAccessToken(user),
                    RefreshToken = refreshToken
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }

        return RefreshTokenResult.Unauthorized;
    }

    public string GenerateRefreshToken(Users user, bool saveToken = true)
    {
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddDays(RefreshTokenTimeout),
            SigningCredentials = refreshTokenKey
        };
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        if (saveToken)
        {
            var userToken = new UserTokens
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginProvider = Guid.NewGuid().ToString(),
                Name = RefreshTokenName,
                Value = token,
                ExpiredAt = DateTime.UtcNow.AddDays(RefreshTokenTimeout)
            };
            context.UserTokens.Add(userToken);
            context.SaveChanges();
        }

        return token;
    }

    public string GenerateAccessToken(Users user)
    {
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.Id, user.Id.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
        identity.AddClaim(new Claim(ClaimTypes.UserName, user.UserName!));
        var claims = GetClaimsAsync(user).Result;
        // add claims of user
        foreach (var claim in claims) identity.AddClaim(claim);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddMinutes(AccessTokenTimeout),
            SigningCredentials = accessTokenKey
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public override async Task<IList<Claim>> GetClaimsAsync(Users user)
    {
        var userClaims = await base.GetClaimsAsync(user);
        foreach (var roleName in await GetRolesAsync(user))
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null) continue;
            var roleClaims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims) userClaims.Add(claim);
        }

        return userClaims;
    }

    public async Task<bool> LogoutAsync(string token)
    {
        try
        {
            // validate jwt token first
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = Configuration.RefreshTokenKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);
            // validate jwt token in db
            var userToken = await context.UserTokens.FirstOrDefaultAsync(userToken => userToken.Value == token);
            if (userToken == null) return false;

            context.UserTokens.Remove(userToken);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }

        return false;
    }
}