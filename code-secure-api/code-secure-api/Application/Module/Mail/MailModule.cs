using CodeSecure.Core;

namespace CodeSecure.Application.Module.Mail;

public class MailModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IMailAddUserToProject, MailAddUserToProject>();
        builder.AddScoped<IMailInviteUser, MailInviteUser>();
        builder.AddScoped<IMailRemoveUserFromProject, MailRemoveUserFromProject>();
        builder.AddScoped<IMailResetPassword, MailResetPassword>();
        return builder;
    }
}