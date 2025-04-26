using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project;

public class ProjectModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IProjectAuthorize, ProjectAuthorize>();
        builder.AddScoped<IProjectSettingService, ProjectSettingService>();
        builder.AddScoped<IProjectMemberService, ProjectMemberService>();
        builder.AddScoped<IProjectPackageService, ProjectPackageService>();
        builder.AddScoped<IProjectService, ProjectService>();
        return builder;
    }
}