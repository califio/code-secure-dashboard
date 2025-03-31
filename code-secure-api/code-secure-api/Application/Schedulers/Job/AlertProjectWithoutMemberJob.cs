using CodeSecure.Application.Module.Integration;
using CodeSecure.Core.EntityFramework;
using Quartz;

namespace CodeSecure.Application.Schedulers.Job;

public class AlertProjectWithoutMemberJob(
    AppDbContext dbContext,
    IGlobalAlertManager globalAlertManager,
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
            _ = globalAlertManager.AlertProjectWithoutMember(new AlertProjectWithoutMemberModel
            {
                Project = project
            });
        }
    }
}