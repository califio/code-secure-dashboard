using CodeSecure.Database;
using CodeSecure.Database.Extension;
using CodeSecure.Manager.Integration;
using CodeSecure.Manager.Integration.Model;
using Quartz;

namespace CodeSecure.Scheduler.Job;

public class AlertProjectWithoutMemberJob(
    AppDbContext dbContext,
    IAlertManager alertManager,
    ILogger<AlertProjectWithoutMemberJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Alert Project Without Member");
        // get max 50 to avoid spam
        var result = await dbContext.Projects
            .Where(project => !dbContext.ProjectUsers
                .Any(record => record.ProjectId == project.Id)
            ).OrderBy(project => project.CreatedAt)
            .PageAsync(1, 50);
        foreach (var project in result.Items)
        {
            await alertManager.AlertProjectWithoutMember(new AlertProjectWithoutMemberModel
            {
                ProjectName = project.Name,
                ProjectUrl = $"{Application.Config.FrontendUrl}/#/project/{project.Id}/setting/member",
                ProjectId = project.Id
            });
        }
    }
}