using CodeSecure.Api.Dependency.Model;
using CodeSecure.Api.Dependency.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Dependency;

public class DependencyController(IDependencyService dependencyService) : BaseController
{
    [HttpGet]
    public async Task<List<PackageInfo>> GetPackageDependencies(Guid packageId)
    {
        return await dependencyService.GetPackageDependenciesAsync(packageId);
    }
}