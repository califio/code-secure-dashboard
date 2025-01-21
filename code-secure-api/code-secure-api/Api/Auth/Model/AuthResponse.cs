namespace CodeSecure.Api.Auth.Model;

public record AuthResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public bool? RequireTwoFactor { get; set; }
    public bool? RequireConfirmEmail { get; set; }
}