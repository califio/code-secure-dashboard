using Atlassian.Jira;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Extension;
using CodeSecure.Manager.Project;
using CodeSecure.Manager.Setting;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Integration.TicketTracker.Jira;

public class JiraTicketTracker(
    AppDbContext context,
    JiraSetting jiraSetting,
    IProjectManager projectManager,
    IJiraManager jiraManager,
    ILogger<JiraTicketTracker> logger
) : ITicketTracker
{
    public async Task<TicketResult<Tickets>> CreateTicketAsync(SastTicket request)
    {
        if (jiraSetting.Active)
        {
            try
            {
                var jiraProjectSetting = await projectManager.GetJiraSettingAsync(request.Project.Id);
                string description = request.Finding.Description;
                description += $"\n\n*Repo:* [{request.Project.Name}|{request.Project.RepoUrl}]";
                var sourceType = await projectManager.GetSourceTypeAsync(request.Project.SourceControlId);
                var location = RepoHelpers.UrlByCommit(sourceType, request.Project.RepoUrl, request.Commit,
                    request.Finding.Location!, request.Finding.StartLine, request.Finding.EndLine);
                description += $"\n\n*Location*: [{request.Finding.Location}|{location}]";
                if (!string.IsNullOrEmpty(request.Finding.Snippet))
                {
                    description += $"\n{{code:java}}\n{request.Finding.Snippet}\n{{code}}";
                }

                if (!string.IsNullOrEmpty(request.Finding.Recommendation))
                {
                    description += $"\n\n*Recommendation*\n{request.Finding.Recommendation}";
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
                var issue = await jiraManager.CreateIssueAsync(jiraIssue);
                var ticket = await CreateTicketAsync(issue);
                await UpdateTicketFindingAsync(request.Finding.Id, ticket);
                return TicketResult<Tickets>.Success(ticket);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.StackTrace);
                return TicketResult<Tickets>.Failed(e.Message);
            }
        }

        return TicketResult<Tickets>.Failed("Jira is not active");
    }

    public async Task<TicketResult<Tickets>> CreateTicketAsync(ScaTicket request)
    {
        if (jiraSetting.Active && request.Vulnerabilities.Count > 0)
        {
            try
            {
                var package = request.Package;
                var jiraProjectSetting = await projectManager.GetJiraSettingAsync(request.Project.Id);
                request.Vulnerabilities.Sort((v1, v2) => v2.Severity - v1.Severity);
                var description =
                    $"The package *{package.FullName()}@{package.Version}* currently in use contains known security vulnerabilities that may pose a risk to our system’s security and stability. Below is the list of identified vulnerabilities:\n\n" +
                    "||Name||Severity||";
                foreach (var vulnerability in request.Vulnerabilities)
                {
                    description += $"\n|{vulnerability.Name}|{vulnerability.Severity.ToString().ToUpper()}|";
                }

                description += $"\n\n*Repo:* [{request.Project.Name}|{request.Project.RepoUrl}]";
                description += $"\n\n*Location:* {request.Location}";
                description += $"\n\n*Recommendation*\nUpgrade {package.FullName()}@{package.Version} to version {package.FixedVersion}";
                var result = await jiraManager.CreateIssueAsync(new JiraIssue
                {
                    Title = $"[{request.Project.Name}] Upgrade package {package.FullName()}@{package.Version} to version {package.FixedVersion} at {request.Location}",
                    Description = description,
                    ProjectKey = jiraProjectSetting.ProjectKey,
                    Type = jiraProjectSetting.IssueType,
                });

                var ticket = await CreateTicketAsync(result);
                await UpdateTicketPackageProjectAsync(projectId: request.Project.Id,
                    packageId: request.Package.Id, ticket);
                return TicketResult<Tickets>.Success(ticket);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.StackTrace);
                return TicketResult<Tickets>.Failed(e.Message);
            }
        }

        return TicketResult<Tickets>.Failed("Jira is not active");
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
                Url = $"{jiraSetting.WebUrl}/browse/{issue.Key.Value}"
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