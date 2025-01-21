namespace CodeSecure.Manager.Notification;

public class NotificationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IMailSender, MailSender>();
        builder.AddScoped<INotification, NotificationManager>();
        return builder;
    }
}