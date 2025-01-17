namespace CodeSecure.Manager.Project;

public class ProjectManagerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IProjectManager, ProjectManager>();
        return builder;
    }
}