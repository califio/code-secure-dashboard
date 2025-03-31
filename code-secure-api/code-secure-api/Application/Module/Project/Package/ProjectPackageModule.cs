using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project.Package;

public class ProjectPackageModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICreateTicketPackageHandler, CreateTicketPackageHandler>();
        builder.AddScoped<IDeleteTicketPackageHandler, DeleteTicketPackageHandler>();
        builder.AddScoped<IFindProjectPackageDetailHandler, FindProjectPackageDetailHandler>();
        builder.AddScoped<IFindProjectPackageHandler, FindProjectPackageHandler>();
        builder.AddScoped<IUpdateProjectPackageHandler, UpdateProjectPackageHandler>();
        return builder;
    }
}