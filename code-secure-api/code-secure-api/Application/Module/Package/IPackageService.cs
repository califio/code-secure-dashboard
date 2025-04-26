using CodeSecure.Application.Module.Package.Command;
using CodeSecure.Application.Module.Package.Model;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Package;

public interface IPackageService
{
    Task<PackageDetail> GetPackageByIdAsync(Guid packageId);
    Task<List<PackageInfo>> ListPackageDependencyAsync(Guid packageId);
}

public class PackageService(AppDbContext context): IPackageService
{
    public async Task<PackageDetail> GetPackageByIdAsync(Guid packageId)
    {
        return (await new GetPackageByIdCommand(context).ExecuteAsync(packageId)).GetResult();
    }

    public async Task<List<PackageInfo>> ListPackageDependencyAsync(Guid packageId)
    {
        return (await new ListPackageDependencyCommand(context).ExecuteAsync(packageId)).GetResult();
    }
}