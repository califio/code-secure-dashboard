using CodeSecure.Api.Package.Model;
using CodeSecure.Api.Package.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Package;

public class PackageController(IPackageService packageService) : BaseController
{
    [HttpGet]
    public async Task<List<PackageInfo>> GetPackageDependencies(Guid packageId)
    {
        return await packageService.GetPackageDependenciesAsync(packageId);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<PackageDetail> GetPackageById(Guid id)
    {
        return await packageService.GetPackageByIdAsync(id);
    }
}