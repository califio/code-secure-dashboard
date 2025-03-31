using CodeSecure.Application.Exceptions;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project;

public interface IProjectAuthorize : IAuthorize<Guid>;

public class ProjectAuthorize(AppDbContext context) : IProjectAuthorize
{
    public bool Authorize(Guid projectId, JwtUserClaims user, string permission)
    {
        if (user.HasClaim(PermissionType.Project, permission)) return true;
        return permission switch
        {
            PermissionAction.Read => context.ProjectUsers.Any(member =>
                member.UserId == user.Id && member.ProjectId == projectId),
            PermissionAction.Update => context.ProjectUsers.Any(member =>
                member.UserId == user.Id && member.ProjectId == projectId && member.Role == ProjectRole.Manager),
            _ => throw new AccessDeniedException()
        };
    }
}