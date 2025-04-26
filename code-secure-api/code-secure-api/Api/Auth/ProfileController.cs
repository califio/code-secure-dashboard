using CodeSecure.Application.Module.Auth;
using CodeSecure.Application.Module.Auth.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

public class ProfileController(IAuthService authService) : BaseController
{
    [HttpGet]
    public Task<UserProfile> GetProfile()
    {
        return authService.GetUserProfileAsync(CurrentUser.Id);
    }
}