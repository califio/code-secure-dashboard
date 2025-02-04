namespace CodeSecure.Manager.Integration.Model;

public record ResetPasswordModel
{
    public required string Username { get; set; }
    public required string ResetPasswordUrl { get; set; }
}