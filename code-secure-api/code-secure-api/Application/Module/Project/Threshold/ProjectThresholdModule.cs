using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project.Threshold;

public class ProjectThresholdModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IGetProjectThresholdHandler, GetProjectThresholdHandler>();
        builder.AddScoped<IUpdateProjectThresholdHandler, UpdateProjectThresholdHandler>();
        return builder;
    }
}