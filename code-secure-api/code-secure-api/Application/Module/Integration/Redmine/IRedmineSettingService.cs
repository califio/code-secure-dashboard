using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Integration.Redmine;

public interface IRedmineSettingService
{
    Task<RedmineSetting> GetSettingAsync();
    Task<bool> UpdateSettingAsync(RedmineSetting setting);
    Task<bool> TestConnectionAsync();
}

public class RedmineSettingService(AppDbContext context) : IRedmineSettingService
{
    public Task<RedmineSetting> GetSettingAsync()
    {
        return context.GetRedmineSettingAsync();
    }

    public async Task<bool> UpdateSettingAsync(RedmineSetting setting)
    {
        return await context.UpdateRedmineSettingAsync(setting);
    }

    public async Task<bool> TestConnectionAsync()
    {
        var setting = await GetSettingAsync();
        var redmineClient = new RedmineClient(setting.Url, setting.Token);
        return redmineClient.TestConnection().GetResult();
    }
}