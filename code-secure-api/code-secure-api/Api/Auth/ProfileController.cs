using CodeSecure.Application.Module.Auth;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

public class ProfileController(IGetUserProfileHandler getUserProfileHandler) : BaseController
{
    [HttpGet]
    public async Task<UserProfile> GetProfile()
    {
        var result = await getUserProfileHandler.HandleAsync(CurrentUser().Id);
        return result.GetResult();
    }
}