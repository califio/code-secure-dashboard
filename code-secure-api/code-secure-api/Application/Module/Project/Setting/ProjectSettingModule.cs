using CodeSecure.Application.Module.Project.Setting.Member;
using CodeSecure.Application.Module.Project.Setting.Threshold;
using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project.Setting;

public  class ProjectSettingModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IGeneralSettingProjectService, GeneralSettingProjectService>();
        // member
        builder.AddScoped<ICreateProjectMemberHandler, CreateProjectMemberHandler>();
        builder.AddScoped<IDeleteProjectMemberHandler, DeleteProjectMemberHandler>();
        builder.AddScoped<IFindProjectMemberHandler, FindProjectMemberHandler>();
        builder.AddScoped<IUpdateProjectMemberHandler, UpdateProjectMemberHandler>();
        // threshold
        builder.AddScoped<IGetProjectThresholdHandler, GetProjectThresholdHandler>();
        builder.AddScoped<IUpdateProjectThresholdHandler, UpdateProjectThresholdHandler>();
        return builder;
    }
}