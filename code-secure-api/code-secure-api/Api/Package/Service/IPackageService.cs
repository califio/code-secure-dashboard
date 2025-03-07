using CodeSecure.Api.Package.Model;

namespace CodeSecure.Api.Package.Service;

public interface IPackageService
{
    Task<List<PackageInfo>> GetPackageDependenciesAsync(Guid packageId);
    Task<PackageDetail> GetPackageByIdAsync(Guid packageId);
}