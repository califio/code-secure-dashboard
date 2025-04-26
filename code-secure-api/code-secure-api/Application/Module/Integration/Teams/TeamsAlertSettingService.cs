using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Integration.Teams;

public interface ITeamsAlertSettingService
{
    Task<TeamsAlertSetting> GetSettingAsync();
    Task<bool> UpdateSettingAsync(TeamsAlertSetting alertSetting);
    Task<bool> TestConnectionAsync();
}

public class TeamsAlertSettingService(AppDbContext context) : ITeamsAlertSettingService
{
    public Task<TeamsAlertSetting> GetSettingAsync()
    {
        return context.GetTeamsAlertSettingAsync();
    }

    public async Task<bool> UpdateSettingAsync(TeamsAlertSetting request)
    {
        return await context.UpdateTeamsAlertSettingAsync(request);
    }

    public async Task<bool> TestConnectionAsync()
    {
        var currentSetting = await GetSettingAsync();
        var result = await new TeamsClient(currentSetting.Webhook).TestConnectionAsync();
        return result.GetResult();
    }
}