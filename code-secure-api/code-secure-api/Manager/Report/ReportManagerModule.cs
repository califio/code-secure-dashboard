namespace CodeSecure.Manager.Report;

public class ReportManagerModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IReportManager, ReportManager>();
        return builder;
    }
}