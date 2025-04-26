using CodeSecure.Core;

namespace CodeSecure.Application.Services;

public class ServiceModule: IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddMvcCore().AddRazorViewEngine();
        builder.AddScoped<IRazorRender, RazorRender>();
        builder.AddScoped<ISmtpService, SmtpService>();
        return builder;
    }
}