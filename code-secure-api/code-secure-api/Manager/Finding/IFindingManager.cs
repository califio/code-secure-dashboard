using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Manager.Finding.Model;
using UserClaims = CodeSecure.Authentication.Jwt.UserClaims;

namespace CodeSecure.Manager.Finding;

public interface IFindingManager
{
    Task<Findings?> FindByIdAsync(Guid id);
    Task<Findings?> FindByIdentityAsync(Guid projectId, string identity);
    Task<Page<FindingSummary>> GetFindingAsync(FindingFilter filter, UserClaims actor);
    Task<Findings> CreateAsync(Findings finding);
    Task<Findings> UpdateAsync(Findings finding);
    Task<int> GetSlaAsync(Findings finding);
}