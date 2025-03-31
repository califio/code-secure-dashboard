using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project.Member;

public class ProjectMemberModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICreateProjectMemberHandler, CreateProjectMemberHandler>();
        builder.AddScoped<IDeleteProjectMemberHandler, DeleteProjectMemberHandler>();
        builder.AddScoped<IFindProjectMemberHandler, FindProjectMemberHandler>();
        builder.AddScoped<IUpdateProjectMemberHandler, UpdateProjectMemberHandler>();
        return builder;
    }
}