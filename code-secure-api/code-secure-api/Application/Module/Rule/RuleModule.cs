using CodeSecure.Core;

namespace CodeSecure.Application.Module.Rule;

public class RuleModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IRuleService, RuleService>();
        return builder;
    }
}