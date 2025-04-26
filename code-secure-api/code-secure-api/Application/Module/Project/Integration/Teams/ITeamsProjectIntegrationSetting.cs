using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Core.Utils;
using FluentResults;
using FluentResults.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Integration.Teams;

public interface ITeamsProjectIntegrationSetting
{
    Task<Result<TeamsProjectSetting>> GetSettingAsync(Guid projectId);
    Task<Result<bool>> UpdateSettingAsync(Guid projectId, TeamsProjectSetting setting);
    Task<Result<bool>> TestConnectionAsync(Guid projectId);
}

public class TeamsProjectIntegrationSetting(AppDbContext context) : ITeamsProjectIntegrationSetting
{
    public async Task<Result<TeamsProjectSetting>> GetSettingAsync(Guid projectId)
    {
        var projectSetting = await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == projectId);
        if (projectSetting == null) return Result.Fail("Project not found");
        return projectSetting.GetTeamsAlertSetting();
    }

    public async Task<Result<bool>> UpdateSettingAsync(Guid projectId, TeamsProjectSetting request)
    {
        var projectSetting =
            await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == projectId);
        if (projectSetting == null) return Result.Fail("Project not found");
        var currentSetting =
            JSONSerializer.DeserializeOrDefault(projectSetting.TeamsSetting, new TeamsProjectSetting());
        if (string.IsNullOrEmpty(request.Webhook))
        {
            request.Webhook = currentSetting.Webhook;
        }

        projectSetting.TeamsSetting = JSONSerializer.Serialize(request);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> TestConnectionAsync(Guid projectId)
    {
        return await GetSettingAsync(projectId)
            .Bind(setting => new TeamsClient(setting.Webhook).TestConnectionAsync());
    }
}