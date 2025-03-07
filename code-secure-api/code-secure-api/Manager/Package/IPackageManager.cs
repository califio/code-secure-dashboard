using CodeSecure.Database.Entity;
using CodeSecure.Manager.Package.Model;

namespace CodeSecure.Manager.Package;

public interface IPackageManager
{
    Task<Packages> CreateAsync(Packages package);
    Task<Packages?> FindByPkgIdAsync(string pkgId);
    Task<List<Packages>> GetDependenciesAsync(string pkgId);
    Task<List<Packages>> GetDependenciesAsync(Guid packageId);
    Task AddDependenciesAsync(string pkgId, IEnumerable<string> pkgDependencies);
    Task<List<Vulnerabilities>> GetVulnerabilitiesAsync(string pkgId);
    Task<List<Vulnerabilities>> GetVulnerabilitiesAsync(Guid packageId);
    Task AddVulnerabilityAsync(string pkgId, Vulnerabilities vulnerability);
    Task AddVulnerabilityAsync(Packages package, Vulnerabilities vulnerability);

    Task AddToProjectAsync(Guid projectId, Packages package, string location);
    Task AddToProjectAsync(Guid projectId, string pkgId, string location);
    Task RemoveFromProjectAsync(Guid projectId, Guid packageId, string location);
    Task RemoveFromProjectAsync(Guid projectId, string pkgId, string location);
    Task<List<PackageProject>> FromProjectAsync(Guid projectId);
    Task UpdateRiskLevelAsync(Packages package);
    Task UpdateRiskLevelAsync(string pkgId);
    Task UpdateRiskImpactAsync(Packages package);
    Task UpdateRiskImpactAsync(string pkgId);
    Task UpdateRecommendationAsync(string pkgId);
    Task UpdateRecommendationAsync(Packages package);
}