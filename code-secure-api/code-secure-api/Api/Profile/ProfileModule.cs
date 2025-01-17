using CodeSecure.Api.Profile.Service;

namespace CodeSecure.Api.Profile;

public class ProfileModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddAutoMapper(typeof(ProfileAutoMapper));
        builder.AddScoped<IProfileService, DefaultProfileService>();
        return builder;
    }
}