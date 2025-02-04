using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Integration;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Project;
using Quartz;

namespace CodeSecure.Scheduler.Job;

public class WeeklySecurityAlertJob(
    AppDbContext dbContext,
    IProjectManager projectManager,
    IAlertManager alertManager,
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
                // dependency alert
                var report = await projectManager.DependencyReportAsync(project);
                var total = report.Critical + report.High + report.Medium + report.Low;
                if (total > 0)
                {
                    var model = new DependencyReportModel
                    {
                        RepoUrl = report.RepoUrl,
                        RepoName = report.RepoName,
                        ProjectDependencyUrl = report.ProjectDependencyUrl,
                        Critical = report.Critical,
                        High = report.High,
                        Medium = report.Medium,
                        Low = report.Low,
                        Packages = report.Packages,
                        ProjectId = project.Id
                    };
                    var subject = $"Weekly Security Alert: Vulnerability found in dependencies of {model.RepoName}";
                    await alertManager.AlertVulnerableDependencies(model, subject);
                }
                // Reminder verify unconfirmed finding
                var openFinding = dbContext.Findings
                    .Count(record => record.ProjectId == project.Id 
                                     && record.Status == FindingStatus.Open);
                if (openFinding > 0)
                {
                    await alertManager.AlertNeedsTriageFinding(new NeedsTriageFindingInfoModel
                    {
                        ProjectName = project.Name,
                        NeedsTriage = openFinding,
                        OpenFindingUrl =
                            $"{Application.Config.FrontendUrl}/#/project/{project.Id.ToString()}/finding?status=Open",
                        ProjectId = project.Id
                    });
                }
            }
            page++;
        } while (page < result.PageCount);
        logger.LogInformation("Weekly Security Alert Job: End");
    }
}