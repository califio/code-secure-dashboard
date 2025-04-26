namespace CodeSecure.Application.Module.Auth.Model;

public record SignInResponse
{
    public static readonly SignInResponse NeedTwoFactor = new() { RequireTwoFactor = true };
    public static readonly SignInResponse NeedConfirmEmail = new() { RequireTwoFactor = true };
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool? RequireTwoFactor { get; set; }
    public bool? RequireConfirmEmail { get; set; }
}