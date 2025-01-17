namespace CodeSecure.Api.Auth.Model;

public record OidcSignInResult
{
    public required AuthResponse? AuthResponse { get; set; }
    public required string? Message { get; set; }
}