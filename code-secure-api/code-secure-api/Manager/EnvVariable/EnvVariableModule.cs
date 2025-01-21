namespace CodeSecure.Manager.EnvVariable;

public class EnvVariableModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IEnvVariableManager, EnvVariableManager>();
        return builder;
    }
}