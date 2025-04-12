using System.Text;
using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using Microsoft.Extensions.Caching.Memory;

namespace CodeSecure.Application.Module.Integration.JiraWebhook;

public interface IJiraWebHookService
{
    Task AlertScanCompleteAsync(AlertScanCompleteModel model);
}

public class JiraWebHookService(AppDbContext context, ILogger<JiraWebHookService> logger) : IJiraWebHookService
{
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());
    private const int ExpiredTime = 10; // minus

    public async Task AlertScanCompleteAsync(AlertScanCompleteModel model)
    {
        logger.LogInformation($"[Jira WebHook] {model.Scanner.Name} scan completed on project \"{model.Project.Name}\"");
        try
        {
            var setting = await context.GetJiraWebhookSettingAsync();
            if (setting.Active)
            {
                var jiraIssueId = model.GitCommit.CommitTitle?.JiraIssueId();
                if (string.IsNullOrEmpty(jiraIssueId))
                {
                    return;
                }
                var message = $"*Repo:* [{model.Project.Name}|{model.Project.RepoUrl}]";
                message += $"\n\n*Commit:* [{model.GitCommit.CommitTitle}|{model.CommitUrl()}]";
                if (model.GitCommit.Type == CommitType.MergeRequest &&
                    !string.IsNullOrEmpty(model.GitCommit.MergeRequestId))
                {
                    var mergeRequestUrl = GitRepoHelpers.BuildMergeRequestUrl(model.SourceType, model.Project.RepoUrl,
                        model.GitCommit.MergeRequestId);
                    message += $"\n\n*Merge Request:* [{model.GitCommit.MergeRequestId}|{mergeRequestUrl}]";
                }

                var otherScans = GetOtherScanCompleteInCommit(model.GitCommit);
                foreach (var otherScan in otherScans)
                {
                    message += $"\n\n{ResultString(otherScan)}";
                }
                CacheAlertScanCompleteModel(model);
                message += $"\n\n{ResultString(model)}";
                var body = $"{{\"issues\":[\"{jiraIssueId}\"], \"data\": {{\"message\":\"{message}\"}}}}";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Automation-Webhook-Token", setting.Token);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                await client.PostAsync(setting.Webhook, content);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
    }

    private static void CacheAlertScanCompleteModel(AlertScanCompleteModel model)
    {
        var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredTime));
        if (model.GitCommit.CommitHash != null)
        {
            if (Cache.TryGetValue(model.GitCommit.CommitHash, out Dictionary<Guid, AlertScanCompleteModel>? mScan) &&
                mScan != null)
            {
                mScan[model.Scanner.Id] = model;
            }
            else
            {
                mScan = new Dictionary<Guid, AlertScanCompleteModel> { { model.Scanner.Id, model } };
            }

            Cache.Set(model.GitCommit.CommitHash, mScan, options);
        }
    }

    private List<AlertScanCompleteModel> GetOtherScanCompleteInCommit(GitCommits commit)
    {
        if (commit.CommitHash != null)
        {
            if (Cache.TryGetValue(commit.CommitHash, out Dictionary<Guid, AlertScanCompleteModel>? mScan) &&
                mScan != null)
            {
                return mScan.Values.ToList();
            }
        }

        return [];
    }

    private string ResultString(AlertScanCompleteModel model)
    {
        var message = $"\n*{model.Scanner.Name}*";
        if (model.NewFindingCount > 0)
        {
            message +=
                $"\nFound {model.NewFindingCount} new finding. [View Detail|{model.FindingUrlByStatus(FindingStatus.Open)}]";
        }
        else
        {
            message = $"\nNo new finding";
        }

        return message;
    }
}