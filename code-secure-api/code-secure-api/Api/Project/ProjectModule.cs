using CodeSecure.Api.Project.Service;

namespace CodeSecure.Api.Project;

public class ProjectModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IProjectService, DefaultProjectService>();
        return builder;
    }
}