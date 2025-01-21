using CodeSecure.Database.Extension;
using CodeSecure.Manager.EnvVariable.Model;
using EnvironmentName = CodeSecure.Database.Entity.EnvironmentName;

namespace CodeSecure.Manager.EnvVariable;

public interface IEnvVariableManager
{
    Task<EnvironmentName?> FindByNameAsync(string name);
    Task<Page<string>> FilterAsync(QueryFilter filter);
    Task CreateAsync(string name);
    Task RemoveAsync(string name);
    Task<Page<EnvironmentVariable>> GetProjectEnvironmentAsync(Guid projectId, QueryFilter filter);
    Task<EnvironmentVariable> SetProjectEnvironmentAsync(Guid projectId, string name, string value);
    Task RemoveProjectEnvironmentAsync(Guid projectId, string name);
}