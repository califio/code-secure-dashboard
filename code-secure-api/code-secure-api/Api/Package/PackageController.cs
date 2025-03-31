using CodeSecure.Application.Module.Package;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Package;

public class PackageController(
    IFindPackageByIdHandler findPackageByIdHandler,
    IListPackageDependencyHandler listPackageDependencyHandler
) : BaseController
{
    [HttpGet]
    [Route("{packageId:guid}/dependencies")]
    public async Task<List<PackageInfo>> GetPackageDependencies(Guid packageId)
    {
        var result = await listPackageDependencyHandler.HandleAsync(packageId);
        return result.GetResult();
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<PackageDetail> GetPackageById(Guid id)
    {
        var result = await findPackageByIdHandler.HandleAsync(id);
        return result.GetResult();
    }
}