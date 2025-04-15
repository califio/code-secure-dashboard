using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Jira.Client;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using FluentResults;
using FluentResults.Extensions;

namespace CodeSecure.Application.Module.Project.Integration.Jira;

public interface IJiraProjectIntegrationSetting
{
    Task<Result<JiraProjectSettingResponse>> GetSettingAsync(Guid projectId, bool reload = false);
    Task<Result<bool>> UpdateSettingAsync(Guid projectId, JiraProjectSetting setting);
}

public class JiraProjectIntegrationSetting(
    AppDbContext context,
    JiraSetting jiraSetting,
    IJiraSettingService jiraSettingService
) : IJiraProjectIntegrationSetting
{
    private readonly JiraClient jiraClient = new(new JiraConnection
    {
        Url = jiraSetting.WebUrl,
        Password = jiraSetting.Password,
        Username = jiraSetting.UserName
    });
    public async Task<Result<JiraProjectSettingResponse>> GetSettingAsync(Guid projectId, bool reload = false)
    {
        return await context.GetProjectSettingsAsync(projectId)
            .Bind(async projectSetting =>
            {
                var globalSetting = await jiraSettingService.GetSettingAsync();
                var jiraSetting = projectSetting.GetJiraSetting(globalSetting);
                return Result.Ok(new JiraProjectSettingResponse
                {
                    Active = jiraSetting.Active,
                    ProjectKey = jiraSetting.ProjectKey,
                    JiraProjects = await jiraClient.GetProjectsSummaryAsync(reload),
                    IssueType = jiraSetting.IssueType
                });
            });
    }

    public async Task<Result<bool>> UpdateSettingAsync(Guid projectId, JiraProjectSetting request)
    {
        return await context.GetProjectSettingsAsync(projectId)
            .Bind(async projectSetting =>
            {
                if (await jiraClient.GetProjectAsync(request.ProjectKey) == null)
                {
                    return Result.Fail("Jira project not found");
                }

                projectSetting.JiraSetting = JSONSerializer.Serialize(request);
                context.ProjectSettings.Update(projectSetting);
                await context.SaveChangesAsync();
                return Result.Ok(true);
            });
    }
}