using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Integration.Jira.Client;

namespace CodeSecure.Application.Module.Integration.Jira;

public interface IJiraSettingService
{
    Task<JiraSetting> GetSettingAsync();
    Task<bool> UpdateSettingAsync(JiraSetting setting);
    Task<bool> TestConnectionAsync();
}

public class JiraSettingService(AppDbContext context) : IJiraSettingService
{
    public Task<JiraSetting> GetSettingAsync()
    {
        return context.GetJiraSettingAsync();
    }

    public async Task<bool> UpdateSettingAsync(JiraSetting request)
    {
        return await context.UpdateJiraSettingAsync(request);
    }

    public async Task<bool> TestConnectionAsync()
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
            throw new BadRequestException(e.Message);
        }
    }
}