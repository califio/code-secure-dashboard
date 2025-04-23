using CodeSecure.Application.Helpers;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Application.Module.SourceControl;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Redmine.Net.Api.Types;

namespace CodeSecure.Application.Module.Integration.Redmine;

public class RedmineTicketTracker : ITicketTracker
{
    private readonly AppDbContext context;
    private readonly RedmineSetting globalSetting;
    private readonly IRedmineClient redmineClient;

    public RedmineTicketTracker(AppDbContext context)
    {
        this.context = context;
        globalSetting = context.GetRedmineSettingAsync().Result;
        redmineClient = new RedmineClient(globalSetting.Url, globalSetting.Token);
    }

    public async Task<Result<Tickets>> CreateTicketAsync(SastTicket request)
    {
        if (globalSetting.Active)
        {
            try
            {
                var projectSetting =
                    (await context.GetProjectSettingsAsync(request.Project.Id)).Value.GetRedmineSetting(globalSetting);
                string description = request.Finding.Description;
                description += $"\n\n**Repo:** [{request.Project.Name}]({request.Project.RepoUrl})";
                var sourceType = (await context.FindSourceControlsByIdAsync(request.Project.SourceControlId)).Value
                    .Type;
                var location = GitRepoHelpers.UrlByCommit(sourceType, request.Project.RepoUrl, request.Commit,
                    request.Finding.Location!, request.Finding.StartLine, request.Finding.EndLine);
                description += $"\n\n**Location**: [{request.Finding.Location}]({location})";
                if (!string.IsNullOrEmpty(request.Finding.Snippet))
                {
                    description += $"\n```\n{request.Finding.Snippet}\n```";
                }

                if (!string.IsNullOrEmpty(request.Finding.Recommendation))
                {
                    description += $"\n\n**Recommendation**\n{request.Finding.Recommendation}";
                }

                description += $"\n\n**Found by:** {request.Scanner.Name}";
                var dueDate = request.Finding.FixDeadline ?? DateTime.Now.AddDays(14);
                var issue = await redmineClient.CreateIssueAsync(new Issue
                {
                    Subject = $"[{request.Project.Name}] {request.Finding.Name}",
                    Description = description,
                    DueDate = dueDate,
                    Project = IdentifiableName.Create<IdentifiableName>(projectSetting.ProjectId),
                    Status = IdentifiableName.Create<IdentifiableName>(projectSetting.StatusId),
                    Tracker = IdentifiableName.Create<IdentifiableName>(projectSetting.TrackerId),
                    Priority = IdentifiableName.Create<IdentifiableName>(projectSetting.PriorityId),
                });
                var ticket = await CreateTicketAsync(issue);
                await UpdateTicketFindingAsync(request.Finding.Id, ticket);
                return ticket;
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        return Result.Fail("Redmine is not active");
    }

    public async Task<Result<Tickets>> CreateTicketAsync(ScaTicket request)
    {
        if (globalSetting.Active && request.Vulnerabilities.Count > 0)
        {
            try
            {
                var package = request.Package;
                var jiraProjectSetting = (await context.GetProjectSettingsAsync(request.Project.Id))
                    .Value.GetRedmineSetting(globalSetting);
                request.Vulnerabilities.Sort((v1, v2) => v2.Severity - v1.Severity);
                var description =
                    $"The package **{package.FullName()}@{package.Version}** currently in use contains known security vulnerabilities that may pose a risk to our system’s security and stability. Below is the list of identified vulnerabilities:\n\n";
                description += "|Name |Severity |Fix Version |\n";
                description += "|-- |-- |-- |\n";
                foreach (var vulnerability in request.Vulnerabilities)
                {
                    description +=
                        $"|{vulnerability.Name}|{vulnerability.Severity.ToString().ToUpper()}|{vulnerability.FixedVersion}|\n";
                }

                description += $"\n\n**Repo:** [{request.Project.Name}]({request.Project.RepoUrl})";
                description += $"\n\n**Location:** {request.Location}";
                description +=
                    $"\n\n**Recommendation**\nUpgrade {package.FullName()}@{package.Version} to version {package.FixedVersion}";
                var dueDate = DateTime.Now.AddDays(30);
                var issue = await redmineClient.CreateIssueAsync(new Issue
                {
                    Subject =
                        $"[{request.Project.Name}] Upgrade package {package.FullName()}@{package.Version} to version {package.FixedVersion} at {request.Location}",
                    Description = description,
                    DueDate = dueDate,
                    Project = IdentifiableName.Create<IdentifiableName>(jiraProjectSetting.ProjectId),
                    Status = IdentifiableName.Create<IdentifiableName>(jiraProjectSetting.StatusId),
                    Tracker = IdentifiableName.Create<IdentifiableName>(jiraProjectSetting.TrackerId),
                    Priority = IdentifiableName.Create<IdentifiableName>(jiraProjectSetting.PriorityId),
                });

                var ticket = await CreateTicketAsync(issue);
                await UpdateTicketPackageProjectAsync(projectId: request.Project.Id, packageId: request.Package.Id,
                    ticket);
                return ticket;
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        return Result.Fail("Redmine is not active");
    }


    private async Task<Tickets> CreateTicketAsync(Issue issue)
    {
        var ticket = context.Tickets.FirstOrDefault(record =>
            record.Type == TicketType.Redmine && record.Name == issue.Id.ToString());
        if (ticket == null)
        {
            ticket = new Tickets
            {
                Name = issue.Id.ToString(),
                Type = TicketType.Redmine,
                Url = $"{globalSetting.Url}/issues/{issue.Id}"
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