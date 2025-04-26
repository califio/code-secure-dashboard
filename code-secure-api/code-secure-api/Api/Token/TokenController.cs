using CodeSecure.Application.Module.Token;
using CodeSecure.Application.Module.Token.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Token;

public class TokenController(
    ITokenService tokenService
) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.CiToken, PermissionAction.Read)]
    public Task<List<CiTokens>> GetCiTokens()
    {
        return tokenService.ListTokensAsync();
    }

    [HttpPost]
    [Permission(PermissionType.CiToken, PermissionAction.Create)]
    public Task<CiTokens> CreateCiToken(CreateTokenRequest request)
    {
        return tokenService.CreateTokenAsync(request);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Permission(PermissionType.CiToken, PermissionAction.Delete)]
    public Task<bool> DeleteCiToken(Guid id)
    {
        return tokenService.DeleteTokenAsync(id);
    }
}