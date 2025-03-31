using CodeSecure.Core;

namespace CodeSecure.Application.Module.Auth;

public class AuthModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IPasswordSignInHandler, PasswordSignInHandler>();
        builder.AddScoped<IOpenIdConnectSignInHandler, OpenIdConnectSignInHandler>();
        builder.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();
        builder.AddScoped<IForgotPasswordHandler, ForgotPasswordHandler>();
        builder.AddScoped<IConfirmEmailHandler, ConfirmEmailHandler>();
        builder.AddScoped<IResetPasswordHandler, ResetPasswordHandler>();
        builder.AddScoped<ILogoutHandler, LogoutHandler>();
        builder.AddScoped<IGetUserProfileHandler, GetUserProfileHandler>();
        return builder;
    }
}