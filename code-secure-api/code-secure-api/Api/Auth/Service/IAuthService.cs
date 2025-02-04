using CodeSecure.Api.Auth.Model;

namespace CodeSecure.Api.Auth.Service;

public interface IAuthService
{
    Task<AuthConfig> GetAuthInfoAsync();
    Task<AuthResponse> PasswordSignInAsync(AuthRequest request);
    Task<OidcSignInResult> OidcSignInAsync(string username);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> LogoutAsync(string refreshToken);
    public Task ForgotPasswordAsync(ForgotPasswordRequest request);
    public Task ResetPasswordAsync(ResetPasswordRequest request);
    public Task<ConfirmEmailResult> ConfirmEmailAsync(ConfirmEmailRequest request);
}