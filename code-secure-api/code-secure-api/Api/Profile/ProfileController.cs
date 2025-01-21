using CodeSecure.Api.Profile.Model;
using CodeSecure.Api.Profile.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Profile;

public class ProfileController(IProfileService profileService) : BaseController
{
    [HttpGet]
    public async Task<UserProfile> GetProfile()
    {
        return await profileService.GetProfileAsync();
    }

    [HttpPost]
    public async Task<UserProfile> UpdateProfile(UpdateProfileRequest request)
    {
        return await profileService.UpdateProfileAsync(request);
    }
}