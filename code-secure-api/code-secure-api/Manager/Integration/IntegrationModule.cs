using CodeSecure.Manager.Integration.Mail;
using CodeSecure.Manager.Integration.Teams;

namespace CodeSecure.Manager.Integration;

public class IntegrationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IMailSender, MailSender>();
        builder.AddScoped<TeamsAlert>();
        builder.AddScoped<MailAlert>();
        builder.AddScoped<IAlertManager, AlertManager>();
        return builder;
    }
}