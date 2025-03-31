using CodeSecure.Application.Module.Integration.Jira.Client;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Jira;

public interface IJiraSettingService
{
    Task<JiraSetting> GetSettingAsync();
    Task<Result<bool>> UpdateSettingAsync(JiraSetting setting);
    Task<Result<bool>> TestConnectionAsync();
}

public class JiraSettingService(AppDbContext context) : IJiraSettingService
{
    public Task<JiraSetting> GetSettingAsync()
    {
        return context.GetJiraSettingAsync();
    }

    public async Task<Result<bool>> UpdateSettingAsync(JiraSetting request)
    {
        return await context.UpdateJiraSettingAsync(request);
    }

    public async Task<Result<bool>> TestConnectionAsync()
    {
        try
        {
            var jiraSetting = await GetSettingAsync();
            var jiraClient = new JiraClient(new JiraConnection
            {
                Url = jiraSetting.WebUrl,
                Username = jiraSetting.UserName,
                Password = jiraSetting.Password,
            });
            await jiraClient.TestConnection();
            return true;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}