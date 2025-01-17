using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Finding;

public interface IFindingManager
{
    Task<Findings?> FindByIdAsync(Guid id);
    Task<Findings?> FindByIdentityAsync(Guid projectId, string identity);
    Task<Findings> CreateAsync(Findings finding);
    Task<Findings> UpdateAsync(Findings finding);
    Task<int> GetSlaAsync(Findings finding);
}