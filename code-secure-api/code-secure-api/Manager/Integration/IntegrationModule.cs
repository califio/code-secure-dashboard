using CodeSecure.Manager.Integration.Mail;

namespace CodeSecure.Manager.Integration;

public class IntegrationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IMailSender, MailSender>();
        builder.AddScoped<IAlert, AlertManager>();
        return builder;
    }
}