using CodeSecure.Api.Profile.Model;
using CodeSecure.Database.Entity;

namespace CodeSecure.Api.Profile;

public class ProfileAutoMapper: AutoMapper.Profile
{
    public ProfileAutoMapper()
    {
        CreateMap<Users, UserProfile>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}