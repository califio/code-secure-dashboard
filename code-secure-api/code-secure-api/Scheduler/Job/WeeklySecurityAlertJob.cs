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
    IAlert alert,
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
                var members = (await projectManager.GetMembersAsync(project.Id))
                    .FindAll(member => member.Status == UserStatus.Active);
                var developers = members.FindAll(member => member.Role == ProjectRole.Developer)
                    .Select(member => member.Email)
                    .ToList();
                var triagers = members.FindAll(member => member.Role != ProjectRole.Developer)
                    .Select(member => member.Email)
                    .ToList();
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
                        Packages = report.Packages
                    };
                    var subject = $"Weekly Security Alert: Vulnerability found in dependencies of {model.RepoName}";
                    alert.PushDependencyReport(developers, model, subject);
                }
                // Reminder verify unconfirmed finding
                var openFinding = dbContext.Findings
                    .Count(record => record.ProjectId == project.Id 
                                     && record.Status == FindingStatus.Open);
                if (openFinding > 0)
                {
                    alert.AlertNeedsTriageFinding(triagers, new NeedsTriageFindingInfoModel
                    {
                        ProjectName = project.Name,
                        NeedsTriage = openFinding,
                        OpenFindingUrl = $"{Application.Config.FrontendUrl}/#/project/{project.Id.ToString()}/finding?status=Open"
                    });
                }
            }
            page++;
        } while (page < result.PageCount);
        logger.LogInformation("Weekly Security Alert Job: End");
    }
}