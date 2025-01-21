namespace CodeSecure.Manager.Notification.Model;

public record MailModel
{
    public required string Subject { get; set; }
    public required IEnumerable<string> Receivers { get; set; }
    public required string Template { get; set; }
    public required object? Model { get; set; }
}