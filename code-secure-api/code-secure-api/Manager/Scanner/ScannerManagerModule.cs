namespace CodeSecure.Manager.Scanner;

public class ScannerManagerModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IScannerManager, ScannerManager>();
        return builder;
    }
}