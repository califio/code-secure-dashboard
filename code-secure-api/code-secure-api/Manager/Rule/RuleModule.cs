namespace CodeSecure.Manager.Rule;

public class RuleModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IRuleManager, RuleManager>();
        return builder;
    }
}