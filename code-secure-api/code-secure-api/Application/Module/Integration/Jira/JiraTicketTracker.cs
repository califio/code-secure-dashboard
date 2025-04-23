using Atlassian.Jira;
using CodeSecure.Application.Helpers;
using CodeSecure.Application.Module.Integration.Jira.Client;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Application.Module.SourceControl;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Integration.Jira;

public class JiraTicketTracker(
    AppDbContext context,
    JiraSetting jiraGlobalSetting,
    ILogger<JiraTicketTracker> logger
) : ITicketTracker
{
    private readonly IJiraClient jiraClient = new JiraClient(new JiraConnection
    {
        Url = jiraGlobalSetting.WebUrl,
        Password = jiraGlobalSetting.Password,
        Username = jiraGlobalSetting.UserName
    });

    public async Task<Result<Tickets>> CreateTicketAsync(SastTicket request)
    {
        if (jiraGlobalSetting.Active)
        {
            try
            {
                var jiraProjectSetting = (await context.GetProjectSettingsAsync(request.Project.Id))
                    .Value.GetJiraSetting(jiraGlobalSetting);
                string description = Converter.MarkdownToJira(request.Finding.Description);
                description += $"\n\n*Repo:* [{request.Project.Name}|{request.Project.RepoUrl}]";
                var sourceType = (await context.FindSourceControlsByIdAsync(request.Project.SourceControlId)).Value
                    .Type;
                var location = GitRepoHelpers.UrlByCommit(sourceType, request.Project.RepoUrl, request.Commit,
                    request.Finding.Location!, request.Finding.StartLine, request.Finding.EndLine);
                description += $"\n\n*Location*: [{request.Finding.Location}|{location}]";
                if (!string.IsNullOrEmpty(request.Finding.Snippet))
                {
                    description += $"\n{{code:java}}\n{request.Finding.Snippet}\n{{code}}";
                }

                if (!string.IsNullOrEmpty(request.Finding.Recommendation))
                {
                    description += $"\n\n*Recommendation*\n{Converter.MarkdownToJira(request.Finding.Recommendation)}";
                }

                description += $"\n\n*Found by:* {request.Scanner.Name}";
                var jiraIssue = new JiraIssue
                {
                    Title = $"[{request.Project.Name}] {request.Finding.Name}",
                    Description = description,
                    DueDate = request.Finding.FixDeadline,
                    ProjectKey = jiraProjectSetting.ProjectKey,
                    Type = jiraProjectSetting.IssueType
                };
                var issue = await jiraClient.CreateIssueAsync(jiraIssue);
                var ticket = await CreateTicketAsync(issue);
                await UpdateTicketFindingAsync(request.Finding.Id, ticket);
                return ticket;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return Result.Fail(e.Message);
            }
        }

        return Result.Fail("Jira is not active");
    }

    public async Task<Result<Tickets>> CreateTicketAsync(ScaTicket request)
    {
        if (jiraGlobalSetting.Active && request.Vulnerabilities.Count > 0)
        {
            try
            {
                var package = request.Package;
                var jiraProjectSetting = (await context.GetProjectSettingsAsync(request.Project.Id))
                    .Value.GetJiraSetting(jiraGlobalSetting);
                request.Vulnerabilities.Sort((v1, v2) => v2.Severity - v1.Severity);
                var description =
                    $"The package *{package.FullName()}@{package.Version}* currently in use contains known security vulnerabilities that may pose a risk to our system’s security and stability. Below is the list of identified vulnerabilities:\n\n" +
                    "||Name||Severity||Fix Version||";
                foreach (var vulnerability in request.Vulnerabilities)
                {
                    description +=
                        $"\n|{vulnerability.Name}|{vulnerability.Severity.ToString().ToUpper()}|{vulnerability.FixedVersion}|";
                }

                description += $"\n\n*Repo:* [{request.Project.Name}|{request.Project.RepoUrl}]";
                description += $"\n\n*Location:* {request.Location}";
                description +=
                    $"\n\n*Recommendation*\nUpgrade {package.FullName()}@{package.Version} to version {package.FixedVersion}";
                var result = await jiraClient.CreateIssueAsync(new JiraIssue
                {
                    Title =
                        $"[{request.Project.Name}] Upgrade package {package.FullName()}@{package.Version} to version {package.FixedVersion} at {request.Location}",
                    Description = description,
                    ProjectKey = jiraProjectSetting.ProjectKey,
                    Type = jiraProjectSetting.IssueType,
                });

                var ticket = await CreateTicketAsync(result);
                await UpdateTicketPackageProjectAsync(projectId: request.Project.Id,
                    packageId: request.Package.Id, ticket);
                return ticket;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return Result.Fail(e.Message);
            }
        }

        return Result.Fail("Jira is not active");
    }

    private async Task<Tickets> CreateTicketAsync(Issue issue)
    {
        var ticket =
            context.Tickets.FirstOrDefault(record => record.Type == TicketType.Jira && record.Name == issue.Key.Value);
        if (ticket == null)
        {
            ticket = new Tickets
            {
                Name = issue.Key.Value,
                Type = TicketType.Jira,
                Url = $"{jiraGlobalSetting.WebUrl}/browse/{issue.Key.Value}"
            };
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
        }

        return ticket;
    }

    private async Task UpdateTicketPackageProjectAsync(Guid projectId, Guid packageId, Tickets ticket)
    {
        await context.ProjectPackages.Where(record => record.ProjectId == projectId && record.PackageId == packageId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.TicketId, ticket.Id));
    }

    private async Task UpdateTicketFindingAsync(Guid findingId, Tickets ticket)
    {
        await context.Findings.Where(record => record.Id == findingId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.TicketId, ticket.Id));
    }
}