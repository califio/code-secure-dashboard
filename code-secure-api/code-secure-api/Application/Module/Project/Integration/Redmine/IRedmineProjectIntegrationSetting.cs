using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Core.Utils;
using FluentResults;
using FluentResults.Extensions;

namespace CodeSecure.Application.Module.Project.Integration.Redmine;

public interface IRedmineProjectIntegrationSetting
{
    Task<Result<RedmineProjectSetting>> GetSettingAsync(Guid projectId);
    Task<Result<bool>> UpdateSettingAsync(Guid projectId, RedmineProjectSetting setting);
    Task<Result<RedmineMetadata>> GetRedmineMetadataAsync(bool reload);
}

public class RedmineProjectIntegrationSetting(AppDbContext context) : IRedmineProjectIntegrationSetting
{
    public async Task<Result<RedmineProjectSetting>> GetSettingAsync(Guid projectId)
    {
        return await context.GetProjectSettingsAsync(projectId)
            .Bind(async projectSetting =>
            {
                var globalSetting = await context.GetRedmineSettingAsync();
                var setting = projectSetting.GetRedmineSetting(globalSetting);
                return Result.Ok(new RedmineProjectSetting
                {
                    Active = setting.Active,
                    ProjectId = setting.ProjectId,
                    StatusId = setting.StatusId,
                    TrackerId = setting.TrackerId,
                    PriorityId = setting.PriorityId,
                });
            });
    }

    public async Task<Result<bool>> UpdateSettingAsync(Guid projectId, RedmineProjectSetting request)
    {
        var globalSetting = await context.GetRedmineSettingAsync();
        var redmineClient = new RedmineClient(globalSetting.Url, globalSetting.Token);
        var metadata = await redmineClient.GetMetadataAsync(false);
        return await context.GetProjectSettingsAsync(projectId)
            .Bind(async projectSetting =>
            {
                // project
                if (metadata.Projects.Any(x => x.Id == request.ProjectId) == false)
                {
                    return Result.Fail("Redmine project not found");
                }

                // tracker
                if (metadata.Trackers.Any(x => x.Id == request.TrackerId) == false)
                {
                    return Result.Fail("Tracker not found");
                }

                // status
                if (metadata.Statuses.Any(x => x.Id == request.StatusId) == false)
                {
                    return Result.Fail("Status invalid");
                }

                // priority
                if (metadata.Priorities.Any(x => x.Id == request.PriorityId) == false)
                {
                    return Result.Fail("Priority invalid");
                }

                projectSetting.RedmineSetting = JSONSerializer.Serialize(request);
                context.ProjectSettings.Update(projectSetting);
                await context.SaveChangesAsync();
                return Result.Ok(true);
            });
    }

    public async Task<Result<RedmineMetadata>> GetRedmineMetadataAsync(bool reload)
    {
        var globalSetting = await context.GetRedmineSettingAsync();
        var redmineClient = new RedmineClient(globalSetting.Url, globalSetting.Token);
        return await redmineClient.GetMetadataAsync(reload);
    }
}