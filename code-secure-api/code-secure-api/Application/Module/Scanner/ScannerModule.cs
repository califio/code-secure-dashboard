using CodeSecure.Core;

namespace CodeSecure.Application.Module.Scanner;

public class ScannerModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IListScannerHandler, ListScannerHandler>();
        return builder;
    }
}