using CodeSecure.Application.Module.Integration.Teams.Client;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Teams;

public interface ITeamsAlertSettingService
{
    Task<TeamsAlertSetting> GetSettingAsync();
    Task<Result<bool>> UpdateSettingAsync(TeamsAlertSetting alertSetting);
    Task<Result<bool>> TestConnectionAsync();
}

public class TeamsAlertSettingService(AppDbContext context) : ITeamsAlertSettingService
{
    public Task<TeamsAlertSetting> GetSettingAsync()
    {
        return context.GetTeamsAlertSettingAsync();
    }

    public async Task<Result<bool>> UpdateSettingAsync(TeamsAlertSetting request)
    {
        return await context.UpdateTeamsAlertSettingAsync(request);
    }

    public async Task<Result<bool>> TestConnectionAsync()
    {
        var currentSetting = await GetSettingAsync();
        return await new TeamsClient(currentSetting.Webhook).TestConnectionAsync();
    }
}