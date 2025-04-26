namespace CodeSecure.Application.Module.Auth.Model;

public record OpenIdConnectSignInResponse
{
    public SignInResponse? AuthResponse { get; set; }
    public string? Message { get; set; }
}