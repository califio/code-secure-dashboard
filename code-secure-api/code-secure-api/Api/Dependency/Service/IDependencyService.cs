using CodeSecure.Api.Dependency.Model;

namespace CodeSecure.Api.Dependency.Service;

public interface IDependencyService
{
    Task<List<PackageInfo>> GetPackageDependenciesAsync(Guid packageId);
}