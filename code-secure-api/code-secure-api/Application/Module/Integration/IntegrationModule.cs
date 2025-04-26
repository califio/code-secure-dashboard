using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Application.Module.Integration.Mail;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Application.Services;
using CodeSecure.Core;

namespace CodeSecure.Application.Module.Integration;

public class IntegrationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        //mail
        builder.AddScoped<IMailAlertSettingService, MailAlertSettingService>();
        builder.AddScoped<MailAlertSetting>(sp =>
        {
            var context = sp.GetRequiredService<AppDbContext>();
            return context.GetMailAlertSettingAsync().Result;
        });
        // teams
        builder.AddScoped<ITeamsAlertSettingService, TeamsAlertSettingService>();
        builder.AddScoped<TeamsAlertSetting>(sp =>
        {
            var context = sp.GetRequiredService<AppDbContext>();
            return context.GetTeamsAlertSettingAsync().Result;
        });
        // jira
        builder.AddScoped<IJiraSettingService, JiraSettingService>();
        builder.AddScoped<JiraTicketTracker>();
        builder.AddScoped<JiraSetting>(sp =>
        {
            var context = sp.GetRequiredService<AppDbContext>();
            return context.GetJiraSettingAsync().Result;
        });
        // jira webhook
        builder.AddScoped<IJiraWebHookService, JiraWebHookService>();
        builder.AddScoped<IJiraWebhookSettingService, JIraWebhookSettingService>();
        // redmine
        builder.AddScoped<IRedmineSettingService, RedmineSettingService>();
        builder.AddScoped<RedmineTicketTracker>();
        // 
        builder.AddScoped<ITicketTrackerManager, TicketTrackerManager>();
        builder.AddScoped<IGlobalAlertManager, GlobalAlertManager>();
        return builder;
    }
}