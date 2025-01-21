namespace CodeSecure.Api.Auth.Model;

public record LogoutRequest
{
    public required string Token { get; set; }
}