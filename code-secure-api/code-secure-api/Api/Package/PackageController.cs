using CodeSecure.Application.Module.Package;
using CodeSecure.Application.Module.Package.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Package;

public class PackageController(
    IPackageService packageService
) : BaseController
{
    [HttpGet]
    [Route("{packageId:guid}/dependencies")]
    public Task<List<PackageInfo>> ListPackageDependency(Guid packageId)
    {
        return packageService.ListPackageDependencyAsync(packageId);
    }

    [HttpGet]
    [Route("{packageId:guid}")]
    public Task<PackageDetail> GetPackageById(Guid packageId)
    {
        return packageService.GetPackageByIdAsync(packageId);
    }
}