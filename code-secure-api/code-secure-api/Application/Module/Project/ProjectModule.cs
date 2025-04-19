using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project;

public class ProjectModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IProjectAuthorize, ProjectAuthorize>();
        builder.AddScoped<IExportFindingHandler, ExportFindingHandler>();
        builder.AddScoped<IFindProjectByIdHandler, FindProjectByIdHandler>();
        builder.AddScoped<IFindProjectHandler, FindProjectHandler>();
        builder.AddScoped<IFindProjectScanHandler, FindProjectScanHandler>();
        builder.AddScoped<IGetStatisticsProjectHandler, GetStatisticsProjectHandler>();
        builder.AddScoped<IListProjectCommitHandler, ListProjectCommitHandler>();
        builder.AddScoped<IGetProjectCommitScanSummary, GetProjectCommitScanSummary>();
        return builder;
    }
}