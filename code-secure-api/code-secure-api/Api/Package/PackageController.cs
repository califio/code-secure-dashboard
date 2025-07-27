using CodeSecure.Application.Module.Package;
using CodeSecure.Application.Module.Package.Model;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Package;

public class PackageController(
    IPackageService packageService
) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public Task<Page<ProjectPackage>> GetPackagesByFilter(PackageFilter filter)
    {
        return packageService.GetPackagesByFilterAsync(filter);
    }
    
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