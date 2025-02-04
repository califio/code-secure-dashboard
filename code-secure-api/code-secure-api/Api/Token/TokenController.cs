using CodeSecure.Api.Token.Model;
using CodeSecure.Api.Token.Service;
using CodeSecure.Authentication;
using CodeSecure.Database.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Token;

public class TokenController(ITokenService tokenService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.CiToken, PermissionAction.Read)]
    public async Task<List<CiTokens>> GetCiTokens()
    {
        return await tokenService.GetCiTokensAsync();
    }

    [HttpPost]
    [Permission(PermissionType.CiToken, PermissionAction.Create)]
    public async Task<CiTokens> CreateCiToken(CreateTokenRequest request)
    {
        return await tokenService.CreateAsync(request);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Permission(PermissionType.CiToken, PermissionAction.Delete)]
    public async Task<bool> DeleteCiToken(Guid id)
    {
        return await tokenService.DeleteAsync(id);
    }
}