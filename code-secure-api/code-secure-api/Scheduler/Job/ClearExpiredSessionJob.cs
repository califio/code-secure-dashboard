using CodeSecure.Database;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace CodeSecure.Scheduler.Job;

public class ClearExpiredSessionJob(AppDbContext dbContext, ILogger<ClearExpiredSessionJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Start clear expired session");
        var count = await dbContext.UserTokens
            .Where(record => record.ExpiredAt != null && record.ExpiredAt < DateTime.UtcNow)
            .ExecuteDeleteAsync();
        logger.LogInformation($"Clear {count} expired session");
    }
}