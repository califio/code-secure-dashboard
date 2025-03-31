using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Services;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace CodeSecure.Application.Schedulers.Job;

public class WeeklySecurityAlertJob(
    AppDbContext dbContext,
    IGlobalAlertManager globalAlertManager,
    IRazorRender razorRender,
    ISmtpService smtpService,
    ILogger<WeeklySecurityAlertJob> logger) : IJob
{
    private const int BulkSize = 1000;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Weekly Security Alert Job: Start");
        var page = 1;
        Page<Projects> result;
        do
        {
            result = dbContext.Projects
                .OrderByDescending(record => record.CreatedAt)
                .Page(page, BulkSize);
            foreach (var project in result.Items)
            {
                var projectAlertManager = new ProjectAlertManager(dbContext, project.Id, smtpService, razorRender);
                // dependency alert
                var packages = dbContext.ProjectPackages
                    .Include(x => x.Package)
                    .Where(x => x.ProjectId == project.Id &&
                                x.Status == PackageStatus.Open &&
                                x.Package!.RiskLevel != RiskLevel.None)
                    .ToList();
                if (packages.Count > 0)
                {
                    var model = new AlertVulnerableProjectPackageModel
                    {
                        Project = project,
                        ProjectPackages = packages
                    };
                    await globalAlertManager.AlertVulnerableProjectPackage(model);
                    await projectAlertManager.AlertVulnerableProjectPackage(model);
                }

                // Reminder verify unconfirmed finding
                var openFinding = dbContext.Findings
                    .Count(record => record.ProjectId == project.Id
                                     && record.Status == FindingStatus.Open);
                if (openFinding > 0)
                {
                    var model = new AlertNeedTriageFindingModel
                    {
                        Project = project,
                        NeedTriageCount = openFinding
                    };
                    await globalAlertManager.AlertNeedTriageFinding(model);
                    await new ProjectAlertManager(dbContext, project.Id, smtpService, razorRender)
                        .AlertNeedTriageFinding(model);
                }
            }

            page++;
        } while (page < result.PageCount);

        logger.LogInformation("Weekly Security Alert Job: End");
    }
}