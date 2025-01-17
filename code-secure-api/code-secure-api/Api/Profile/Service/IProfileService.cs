using CodeSecure.Api.Profile.Model;

namespace CodeSecure.Api.Profile.Service;

public interface IProfileService
{
    Task<UserProfile> GetProfileAsync();
    Task<UserProfile> UpdateProfileAsync(UpdateProfileRequest request);
}