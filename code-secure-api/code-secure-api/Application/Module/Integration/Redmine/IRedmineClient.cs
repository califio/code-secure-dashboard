using System.Collections.Specialized;
using FluentResults;
using Redmine.Net.Api;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Net;
using Redmine.Net.Api.Types;

namespace CodeSecure.Application.Module.Integration.Redmine;

public interface IRedmineClient
{
    Result<bool> TestConnection();
    Task<Result<List<IdName>>> GetProjectsAsync(bool reload);
    Task<Result<List<IdName>>> GetStatusAsync(bool reload);
    Task<Result<List<IdName>>> GetTrackersAsync(bool reload);
    Task<Result<List<IdName>>> GetPrioritiesAsync(bool reload);
    Task<RedmineMetadata> GetMetadataAsync(bool reload);
    Task<Issue> CreateIssueAsync(Issue issue);
}

public class RedmineClient : IRedmineClient
{
    private static List<IdName>? projects;
    private static List<IdName>? statuses;
    private static List<IdName>? trackers;
    private static List<IdName>? priorities;
    private readonly RedmineManager? redmineManager;

    public RedmineClient(string url, string token)
    {
        try
        {
            redmineManager = new(new RedmineManagerOptionsBuilder()
                .WithHost(url)
                .WithApiKeyAuthentication(token)
            );
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public Result<bool> TestConnection()
    {
        try
        {
            var account = redmineManager.GetMyAccount(null);
            return account.Id > 0;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result<List<IdName>>> GetProjectsAsync(bool reload)
    {
        if (redmineManager == null)
        {
            return new Result<List<IdName>>();
        }
        if (reload || projects == null)
        {
            try
            {
                var result = redmineManager.Get<global::Redmine.Net.Api.Types.Project>();
                projects = result.Select(x => new IdName { Name = x.Name, Id = x.Id }).ToList();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        return projects;
    }

    public async Task<Result<List<IdName>>> GetStatusAsync(bool reload)
    {
        if (redmineManager == null)
        {
            return new Result<List<IdName>>();
        }
        if (reload || statuses == null)
        {
            try
            {
                var result = await redmineManager.GetAsync<IssueStatus>();
                statuses = result.Select(x => new IdName { Name = x.Name, Id = x.Id }).ToList();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        return statuses;
    }

    public async Task<Result<List<IdName>>> GetTrackersAsync(bool reload)
    {
        if (redmineManager == null)
        {
            return new Result<List<IdName>>();
        }
        if (reload || trackers == null)
        {
            try
            {
                var result = await redmineManager.GetAsync<Tracker>();
                trackers = result.Select(x => new IdName { Name = x.Name, Id = x.Id }).ToList();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        return trackers;
    }

    public async Task<Result<List<IdName>>> GetPrioritiesAsync(bool reload)
    {
        if (redmineManager == null)
        {
            return new Result<List<IdName>>();
        }
        if (reload || priorities == null)
        {
            try
            {
                var result = await redmineManager.GetAsync<IssuePriority>();
                priorities = result.Select(x => new IdName { Name = x.Name, Id = x.Id }).ToList();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        return priorities;
    }

    public async Task<RedmineMetadata> GetMetadataAsync(bool reload)
    {
        var projectsTask = GetProjectsAsync(reload);
        var statusTask = GetStatusAsync(reload);
        var trackersTask = GetTrackersAsync(reload);
        var prioritiesTask = GetPrioritiesAsync(reload);
        await Task.WhenAll(projectsTask, statusTask, trackersTask, prioritiesTask);
        var results = new[]
        {
            await projectsTask,
            await statusTask,
            await trackersTask,
            await prioritiesTask
        };
        if (results.Any(r => r.IsFailed))
        {
            return new RedmineMetadata
            {
                Projects = [],
                Trackers = [],
                Priorities = [],
                Statuses = []
            };
        }

        return new RedmineMetadata
        {
            Projects = (await projectsTask).Value,
            Statuses = (await statusTask).Value,
            Trackers = (await trackersTask).Value,
            Priorities = (await prioritiesTask).Value
        };
    }

    public async Task<Issue> CreateIssueAsync(Issue issue)
    {
        var parameters = new NameValueCollection
        {
            { "subject", issue.Subject },
            { "limit", "1" },
            { "project_id", issue.Project.Id.ToString() }
        };
        var issues = redmineManager.Get<Issue>(new RequestOptions { QueryString = parameters });
        if (issues == null || issues.Count == 0)
        {
            return await redmineManager.CreateAsync(issue);
        }

        return issues.First();
    }
}