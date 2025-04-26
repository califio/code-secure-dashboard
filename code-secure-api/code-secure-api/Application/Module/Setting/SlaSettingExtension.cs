using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Setting;

public static class SlaSettingExtension
{
    private static SlaSetting? slaSetting;

    public static async Task<SlaSetting> GetSlaSettingAsync(this AppDbContext context)
    {
        if (slaSetting == null)
        {
            var setting = await context.GetAppSettingsAsync();
            slaSetting = new SlaSetting
            {
                Sast = JSONSerializer.DeserializeOrDefault(setting.SlaSastSetting, new SLA()),
                Sca = JSONSerializer.DeserializeOrDefault(setting.SlaScaSetting, new SLA()),
            };
        }

        return slaSetting;
    }

    public static async Task UpdateSlaSettingAsync(this AppDbContext context, SlaSetting request)
    {
        var setting = await context.GetAppSettingsAsync();
        setting.SlaSastSetting = JSONSerializer.Serialize(request.Sast);
        setting.SlaScaSetting = JSONSerializer.Serialize(request.Sca);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        slaSetting = request;
    }
}