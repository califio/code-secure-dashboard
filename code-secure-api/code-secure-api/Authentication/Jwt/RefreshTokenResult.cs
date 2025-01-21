namespace CodeSecure.Authentication.Jwt;

public class RefreshTokenResult
{
    public static readonly RefreshTokenResult Unauthorized = new()
    {
        IsUnauthorized = true,
        Success = false
    };

    public required bool Success { get; set; }
    public bool IsUnauthorized { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}