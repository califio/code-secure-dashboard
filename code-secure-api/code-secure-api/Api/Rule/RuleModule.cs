using CodeSecure.Api.Rule.Service;

namespace CodeSecure.Api.Rule;

public class RuleModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IRuleService, RuleService>();
        return builder;
    }
}