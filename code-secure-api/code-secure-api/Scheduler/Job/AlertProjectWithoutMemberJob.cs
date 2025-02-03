using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Database;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Integration;
using CodeSecure.Manager.Integration.Model;
using Quartz;

namespace CodeSecure.Scheduler.Job;

public class AlertProjectWithoutMemberJob(
    AppDbContext dbContext,
    IAlert alert,
    JwtUserManager userManager, 
    ILogger<AlertProjectWithoutMemberJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Alert Project Without Member");
        var users = await userManager.GetUsersInRoleAsync(RoleDefaults.Admin);
        var receivers = users
            .Where(user => user.Status == UserStatus.Active && user.UserName != "system")
            .Select(user => user.Email).ToList();
        if (receivers.Count != 0)
        {
            // get max 50 to avoid spam
            var result = await dbContext.Projects
                .Where(project => !dbContext.ProjectUsers
                    .Any(record => record.ProjectId == project.Id)
                ).OrderBy(project => project.CreatedAt)
                .PageAsync(1, 50);
            foreach (var project in result.Items)
            {
                alert.AlertProjectWithoutMember(receivers, new AlertProjectWithoutMemberModel
                {
                    ProjectName = project.Name,
                    ProjectUrl = $"{Application.Config.FrontendUrl}/#/project/{project.Id}/setting/member"
                });
            }
        }
        else
        {
            logger.LogWarning("There are no admin account");
        }
        
    }
}