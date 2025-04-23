using CodeSecure.Application.Module.Project.Integration.Jira;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Application.Module.Project.Integration.Redmine;
using CodeSecure.Application.Module.Project.Integration.Teams;
using CodeSecure.Core;

namespace CodeSecure.Application.Module.Project.Integration;

public class ProjectIntegrationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        // jira
        builder.AddScoped<IJiraProjectIntegrationSetting, JiraProjectIntegrationSetting>();
        // teams
        builder.AddScoped<ITeamsProjectIntegrationSetting, TeamsProjectIntegrationSetting>();
        // mail
        builder.AddScoped<IMailProjectIntegrationSetting, MailProjectIntegrationSetting>();
        // redmine
        builder.AddScoped<IRedmineProjectIntegrationSetting, RedmineProjectIntegrationSetting>();
        return builder;
    }
}