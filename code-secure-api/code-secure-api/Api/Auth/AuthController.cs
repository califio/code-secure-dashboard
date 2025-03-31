using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Auth;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Application.Services;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Auth;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[AllowAnonymous]
public class AuthController(
    IAuthSetting authSetting,
    IPasswordSignInHandler passwordSignInHandler,
    IRefreshTokenHandler refreshTokenHandler,
    IForgotPasswordHandler forgotPasswordHandler,
    IResetPasswordHandler resetPasswordHandler,
    IConfirmEmailHandler confirmEmailHandler,
    ILogoutHandler logoutHandler,
    IRazorRender render
) : Controller
{
    [HttpGet]
    [Route("/api/render-mail")]
    public async Task<string> RenderMail()
    {
        var model = new AlertStatusFindingModel
        {
            SourceType = SourceType.GitLab,
            Project = new Projects
            {
                Id = default,
                Metadata = null,
                CreatedAt = default,
                UpdatedAt = null,
                Name = null,
                RepoId = null,
                RepoUrl = null,
                SourceControlId = default,
                SourceControl = null
            },
            Scanner = new Scanners
            {
                Name = "Semgrep",
                Type = ScannerType.Sast,
            },
            GitCommit = new GitCommits
            {
                CommitHash = "cbadbca1231ncabd",
                CommitTitle = "Commit Main",
                Type = CommitType.CommitBranch,
                Branch = "main",
                IsDefault = true,
                TargetBranch = null,
                MergeRequestId = null,
                ProjectId = Guid.NewGuid(),
            },
            Findings = [
                new Findings
                {
                    Id = Guid.NewGuid(),
                    Identity = Guid.NewGuid().ToString(),
                    Name = "Sql Injection 01",
                    Description = "",
                    Category = "SQL Injection",
                    Recommendation = null,
                    Status = FindingStatus.Open,
                    Severity = FindingSeverity.Critical,
                    VerifiedAt = null,
                    FixedAt = null,
                    FixDeadline = null,
                    RuleId = null,
                    Location = null,
                    Snippet = null,
                    StartLine = null,
                    EndLine = null,
                    StartColumn = null,
                    EndColumn = null,
                    ProjectId = Guid.NewGuid(),
                    Project = null,
                    ScannerId = Guid.NewGuid(),
                    Scanner = null,
                    TicketId = null,
                    Ticket = null
                }
            ]
        };
        return await render.RenderAsync("Resources/Templates/AlertNewFinding.cshtml", model);
    }

    [HttpGet]
    [Route("/api/auth-config")]
    public async Task<AuthConfig> GetAuthConfig()
    {
        return await authSetting.GetAuthConfigAsync();
    }

    [HttpPost]
    [Route("/api/login")]
    public async Task<SignInResponse> Login(SignInRequest request)
    {
        var result = await passwordSignInHandler.HandleAsync(request);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }

    [HttpPost]
    [Route("/api/refresh-token")]
    public async Task<SignInResponse> RefreshToken(RefreshTokenRequest request)
    {
        var result = await refreshTokenHandler.HandleAsync(request);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new UnauthorizedException();
    }

    [HttpPost]
    [Route("/api/logout")]
    public async Task<bool> Logout(LogoutRequest request)
    {
        var result = await logoutHandler.HandleAsync(request);
        return result.Value;
    }

    [HttpPost]
    [Route("/api/forgot-password")]
    public async Task ForgotPassword(ForgotPasswordRequest request)
    {
        await forgotPasswordHandler.HandleAsync(request);
    }

    [HttpPost]
    [Route("/api/reset-password")]
    public async Task ResetPassword(ResetPasswordRequest request)
    {
        await resetPasswordHandler.HandleAsync(request);
    }

    [HttpPost]
    [Route("/api/confirm-email")]
    public async Task<ConfirmEmailResponse> ConfirmEmail(ConfirmEmailRequest request)
    {
        var result = await confirmEmailHandler.HandleAsync(request);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }
}
