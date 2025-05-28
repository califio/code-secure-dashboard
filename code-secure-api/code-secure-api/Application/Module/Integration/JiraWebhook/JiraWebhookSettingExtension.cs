using System.Text.RegularExpressions;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Integration.JiraWebhook;

public static class JiraWebhookSettingExtension
{
    private static JiraWebhookSetting? setting;

    public static async Task<JiraWebhookSetting> GetJiraWebhookSettingAsync(this AppDbContext context)
    {
        if (setting == null)
        {
            var appSettings = await context.GetAppSettingsAsync();
            setting = JSONSerializer.DeserializeOrDefault(appSettings.JiraWebhookSetting, new JiraWebhookSetting());
        }

        return setting with { };
    }

    public static async Task<bool> UpdateJiraWebhookSettingAsync(this AppDbContext context, JiraWebhookSetting request)
    {
        var currentSetting = await context.GetJiraWebhookSettingAsync();
        if (string.IsNullOrEmpty(request.Token))
        {
            request.Token = currentSetting.Token;
        }

        var appSettings = await context.GetAppSettingsAsync();
        appSettings.JiraWebhookSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(appSettings);
        await context.SaveChangesAsync();
        setting = request;
        return true;
    }

    public static string? JiraIssueId(this string title)
    {
        string pattern = "([A-Za-z]+-\\d+)";
        var match = Regex.Match(title, pattern);
        if (match.Success)
        {
            return match.Groups[1].Value.ToUpper();
        }

        return null;
    }
}