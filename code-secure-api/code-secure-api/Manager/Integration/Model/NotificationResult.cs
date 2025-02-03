namespace CodeSecure.Manager.Integration.Model;

public class NotificationResult
{
    public static readonly NotificationResult Success = new() { Succeeded = true };
    public required bool Succeeded { get; set; }
    public string Error { get; set; } = string.Empty;

    public static NotificationResult Failed(string error)
    {
        return new NotificationResult
        {
            Succeeded = false,
            Error = error
        };
    }
}