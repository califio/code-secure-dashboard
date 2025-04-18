using System.Text.Json.Serialization;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Package;

public record CreateTicketPackageProjectRequest
{
    public TicketType TicketType { get; set; }
    [JsonIgnore] public Guid ProjectId { get; set; }
    [JsonIgnore] public Guid PackageId { get; set; }
}

public interface ICreateTicketPackageHandler : IHandler<CreateTicketPackageProjectRequest, Tickets>;

public class CreateTicketPackageHandler(AppDbContext context, JiraTicketTracker jiraTicketTracker)
    : ICreateTicketPackageHandler
{
    public async Task<Result<Tickets>> HandleAsync(CreateTicketPackageProjectRequest request)
    {
        var projectPackage = await context.ProjectPackages
            .Include(record => record.Project)
            .Include(record => record.Package)
            .FirstOrDefaultAsync(record =>
                record.ProjectId == request.ProjectId && record.PackageId == request.ProjectId);
        if (projectPackage == null)
        {
            return Result.Fail("Project package not found");
        }

        var vulnerabilities = context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == request.PackageId)
            .Select(record => record.Vulnerability!)
            .ToList();
        if (request.TicketType == TicketType.Jira)
        {
            return await jiraTicketTracker.CreateTicketAsync(new ScaTicket
            {
                Location = projectPackage.Location,
                Project = projectPackage.Project!,
                Package = projectPackage.Package!,
                Vulnerabilities = vulnerabilities
            });
        }

        return Result.Fail("Ticket type not supported");
    }
}