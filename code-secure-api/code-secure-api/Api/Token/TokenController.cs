using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Token;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Token;

public class TokenController(
    ICreateTokenHandler createTokenHandler,
    IDeleteTokenHandler deleteTokenHandler,
    IListTokenHandler listTokenHandler
) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.CiToken, PermissionAction.Read)]
    public async Task<List<CiTokens>> GetCiTokens()
    {
        var result = await listTokenHandler.HandleAsync();
        return result.Value;
    }

    [HttpPost]
    [Permission(PermissionType.CiToken, PermissionAction.Create)]
    public async Task<CiTokens> CreateCiToken(CreateTokenRequest request)
    {
        var result = await createTokenHandler.HandleAsync(request);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Permission(PermissionType.CiToken, PermissionAction.Delete)]
    public async Task<bool> DeleteCiToken(Guid id)
    {
        var result = await deleteTokenHandler.HandleAsync(id);
        if (result.IsFailed)
        {
            throw new BadRequestException(result.Errors.Select(error => error.Message));
        }

        return true;
    }
}