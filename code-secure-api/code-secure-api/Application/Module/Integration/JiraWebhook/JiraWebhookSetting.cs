namespace CodeSecure.Application.Module.Integration.JiraWebhook;

public record JiraWebhookSetting
{
    public bool Active { get; set; }
    public string Webhook { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}