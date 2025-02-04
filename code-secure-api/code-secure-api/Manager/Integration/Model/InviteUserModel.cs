namespace CodeSecure.Manager.Integration.Model;

public record InviteUserModel
{
    public required string Username { get; set; }
    public required string ConfirmUrl { get; set; }
}