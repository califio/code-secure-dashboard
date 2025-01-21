using CodeSecure.Api.Admin.CIToken.Model;
using CodeSecure.Authentication;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Exception;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Admin.CIToken.Service;

public class DefaultCiTokenService(
    AppDbContext context,
    IHttpContextAccessor contextAccessor
) : BaseService<CiTokens>(contextAccessor), ICiTokenService
{
    public async Task<List<CiTokens>> GetCiTokensAsync()
    {
        return await context.CiTokens.OrderByDescending(record => record.CreatedAt).ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var token = await FindByIdAsync(id);
        if (!HasPermission(token, PermissionAction.Delete)) throw new AccessDeniedException();

        context.CiTokens.Remove(token);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<CiTokens> CreateAsync(CreateCiTokenRequest request)
    {
        if (!CurrentUser().HasClaim(PermissionType.CiToken, PermissionAction.Create)) throw new AccessDeniedException();

        var tokenName = request.Name.Trim();
        if (context.CiTokens.Any(record => record.Name == tokenName)) throw new BadRequestException("CI token exists");

        var tokenValue = $"{Guid.NewGuid().ToString()}{Guid.NewGuid().ToString()}".Replace("-", "");
        var token = new CiTokens
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Name = tokenName,
            Value = tokenValue,
            ExpiredAt = null
        };
        context.CiTokens.Add(token);
        await context.SaveChangesAsync();
        return token;
    }

    protected override bool HasPermission(CiTokens entity, string action)
    {
        return CurrentUser().HasClaim(PermissionType.CiToken, action);
    }

    protected override async Task<CiTokens> FindByIdAsync(Guid id)
    {
        return await context.CiTokens.FirstOrDefaultAsync(finding => finding.Id == id) ??
               throw new BadRequestException("CI token not found");
    }
}