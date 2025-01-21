using CodeSecure.Database.Entity;
using CodeSecure.Manager.Project.Model;

namespace CodeSecure.Manager.Project;

public interface IProjectManager
{
    Task<Projects> CreateOrUpdateAsync(Projects input);
    Task<List<Member>> GetMembersAsync(Guid projectId);
    Task<DependencyReport?> DependencyReportAsync(Guid projectId);

    Task<DependencyReport> DependencyReportAsync(Projects project);
}