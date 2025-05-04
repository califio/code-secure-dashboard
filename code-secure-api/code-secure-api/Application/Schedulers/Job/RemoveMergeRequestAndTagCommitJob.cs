using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace CodeSecure.Application.Schedulers.Job;

public class RemoveMergeRequestAndTagCommitJob(AppDbContext dbContext, ILogger<RemoveMergeRequestAndTagCommitJob> logger) : IJob
{
    private const int MaxDays = 30;
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Check and remove merge request and tag commits");
        var commitIds = await dbContext.ProjectCommits
            .Where(x => x.Type == CommitType.MergeRequest)
            .Where(x => x.CreatedAt < DateTime.UtcNow.AddDays(-int.Abs(MaxDays)))
            .Select(x => x.Id)
            .ToListAsync();
        dbContext.ProjectCommits
            .Where(x => x.Type == CommitType.CommitTag)
            .Where(x => x.CreatedAt < DateTime.UtcNow.AddDays(-int.Abs(MaxDays)))
            .Where(x => dbContext.ProjectCommits.Count(pc => x.ProjectId == pc.ProjectId && pc.Type == CommitType.CommitTag) > 1)
            .Select(x => x.Id)
            .ToList().ForEach(commitId =>
            {
                commitIds.Add(commitId);
            });
        foreach (var commitId in commitIds)
        {
            await dbContext.FindingActivities.Where(x => x.CommitId == commitId).ExecuteDeleteAsync();
            await dbContext.ProjectCommits.Where(x => x.Id == commitId).ExecuteDeleteAsync();
        }
        // remove finding not in any scan
        await dbContext.Findings.Where(x => dbContext.ScanFindings.Any(scanFinding => scanFinding.FindingId == x.Id) == false).ExecuteDeleteAsync();
        logger.LogInformation($"Removed {commitIds.Count} merge request and tag commits");
    }
}