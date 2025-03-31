using CodeSecure.Application.Exceptions;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding;

public interface IFindingAuthorize : IAuthorize<Guid>;

public class FindingAuthorize(AppDbContext context) : IFindingAuthorize
{
    public bool Authorize(Guid findingId, JwtUserClaims user, string permission)
    {
        if (user.HasClaim(PermissionType.Finding, permission)) return true;
        var member = context.ProjectUsers.FirstOrDefault(record => 
            record.UserId == user.Id && 
            context.Findings.Any(finding => 
                finding.Id == findingId && 
                finding.ProjectId == record.ProjectId
            )
        );
        if (member == null) return false;

        return permission switch
        {
            PermissionAction.Read => true,
            PermissionAction.Update => member.Role is ProjectRole.Manager or ProjectRole.Validator,
            _ => throw new AccessDeniedException()
        };
    }
}