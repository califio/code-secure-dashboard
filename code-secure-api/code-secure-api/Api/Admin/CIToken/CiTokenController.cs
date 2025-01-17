using CodeSecure.Api.Admin.CIToken.Model;
using CodeSecure.Api.Admin.CIToken.Service;
using CodeSecure.Authentication;
using CodeSecure.Database.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Admin.CIToken;

[Route("api/admin/ci-token")]
public class CiTokenController(ICiTokenService ciTokenService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.CiToken, PermissionAction.Read)]
    public async Task<List<CiTokens>> GetCiTokens()
    {
        return await ciTokenService.GetCiTokensAsync();
    }

    [HttpPost]
    [Permission(PermissionType.CiToken, PermissionAction.Create)]
    public async Task<CiTokens> CreateCiToken(CreateCiTokenRequest request)
    {
        return await ciTokenService.CreateAsync(request);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Permission(PermissionType.CiToken, PermissionAction.Delete)]
    public async Task<bool> DeleteCiToken(Guid id)
    {
        return await ciTokenService.DeleteAsync(id);
    }
}