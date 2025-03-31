using FluentResults;

namespace CodeSecure.Application.Module.Integration.Mail;

public interface IMailAlertSettingService
{
    Task<MailAlertSetting> GetSettingAsync();
    Task<Result<bool>> UpdateSettingAsync(MailAlertSetting request);
}

public class MailAlertSettingService(AppDbContext context) : IMailAlertSettingService
{
    public Task<MailAlertSetting> GetSettingAsync()
    {
        return context.GetMailAlertSettingAsync();
    }

    public async Task<Result<bool>> UpdateSettingAsync(MailAlertSetting request)
    {
        return await context.UpdateMailAlertSettingAsync(request);
    }
}