using CodeSecure.Core;

namespace CodeSecure.Application.Module.Finding;

public class FindingModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IFindingAuthorize, FindingAuthorize>();
        builder.AddScoped<ICreateCommentFindingHandler, CreateCommentFindingHandler>();
        builder.AddScoped<ICreateTicketFindingHandler, CreateTicketFindingHandler>();
        builder.AddScoped<IDeleteTicketFindingHandler, DeleteTicketFindingHandler>();
        builder.AddScoped<IExportFindingHandler, ExportFindingHandler>();
        builder.AddScoped<IFindActivityFindingHandler, FindActivityFindingHandler>();
        builder.AddScoped<IFindFindingByIdHandler, FindFindingByIdHandler>();
        builder.AddScoped<IFindFindingHandler, FindFindingHandler>();
        builder.AddScoped<IListFindingCategoryHandler, ListFindingCategoryHandler>();
        builder.AddScoped<IListFindingRulesHandler, ListFindingRulesHandler>();
        builder.AddScoped<IUpdateFindingHandler, UpdateFindingHandler>();
        builder.AddScoped<IUpdateStatusScanFindingHandler, UpdateStatusScanFindingHandler>();
        return builder;
    }
}