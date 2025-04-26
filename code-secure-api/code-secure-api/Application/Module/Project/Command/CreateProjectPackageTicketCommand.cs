using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class CreateProjectPackageTicketCommand(AppDbContext context)
{
    public async Task<Result<Tickets>> ExecuteAsync(CreateProjectPackageTicketRequest request)
    {
        var projectPackage = await context.ProjectPackages
            .Include(record => record.Project)
            .Include(record => record.Package)
            .FirstOrDefaultAsync(record =>
                record.ProjectId == request.ProjectId && record.PackageId == request.PackageId);
        if (projectPackage == null)
        {
            return Result.Fail("Project package not found");
        }

        var vulnerabilities = context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == request.PackageId)
            .Select(record => record.Vulnerability!)
            .ToList();
        var ticket = new ScaTicket
        {
            Location = projectPackage.Location,
            Project = projectPackage.Project!,
            Package = projectPackage.Package!,
            Vulnerabilities = vulnerabilities
        };
        if (request.TicketType == TicketType.Jira)
        {
            var jiraTicketTracker = new JiraTicketTracker(context);
            return await jiraTicketTracker.CreateTicketAsync(ticket);
        }

        if (request.TicketType == TicketType.Redmine)
        {
            var redmineTicketTracker = new RedmineTicketTracker(context);
            return await redmineTicketTracker.CreateTicketAsync(ticket);
        }

        return Result.Fail("Ticket type not supported");
    }
}