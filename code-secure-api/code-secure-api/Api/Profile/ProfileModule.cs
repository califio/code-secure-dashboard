using CodeSecure.Api.Profile.Service;

namespace CodeSecure.Api.Profile;

public class ProfileModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IProfileService, DefaultProfileService>();
        return builder;
    }
}