namespace CodeSecure.Manager.Integration.Model;

public record AddUserToProjectModel
{
    public required string ProjectName { get; set; }
    public required string ProjectUrl { get; set; }
    public required string Role { get; set; }
    public required string Username { get; set; }
}