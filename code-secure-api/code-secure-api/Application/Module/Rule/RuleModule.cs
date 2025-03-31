using CodeSecure.Core;

namespace CodeSecure.Application.Module.Rule;

public class RuleModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<ICreateRuleHandler, CreateRuleHandler>();
        builder.AddScoped<IListRuleIdHandler, ListRuleIdHandler>();
        builder.AddScoped<IListScannerRuleHandler, ListScannerRuleHandler>();
        builder.AddScoped<IQueryRuleInfoHandler, QueryRuleInfoHandler>();
        builder.AddScoped<IUpdateRuleHandler, UpdateRuleHandler>();
        builder.AddScoped<ISyncRuleHandler, SyncRuleHandler>();
        return builder;
    }
}