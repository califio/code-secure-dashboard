using FluentResults;

namespace CodeSecure.Application.Module.Integration.Redmine;

public interface IRedmineSettingService
{
    Task<RedmineSetting> GetSettingAsync();
    Task<Result<bool>> UpdateSettingAsync(RedmineSetting setting);
    Task<Result<bool>> TestConnectionAsync();
}

public class RedmineSettingService(AppDbContext context): IRedmineSettingService
{
    public Task<RedmineSetting> GetSettingAsync()
    {
        return context.GetRedmineSettingAsync();
    }

    public async Task<Result<bool>> UpdateSettingAsync(RedmineSetting setting)
    {
        return await context.UpdateRedmineSettingAsync(setting);
    }

    public async Task<Result<bool>> TestConnectionAsync()
    {
        var setting = await GetSettingAsync();
        var redmineClient = new RedmineClient(setting.Url, setting.Token);
        return redmineClient.TestConnection();
    }
}