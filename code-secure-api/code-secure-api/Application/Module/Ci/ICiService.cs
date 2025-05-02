using CodeSecure.Application.Module.Ci.Command;
using CodeSecure.Application.Module.Ci.Model;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Ci;

public interface ICiService
{
    Task<CiScanInfo> CreateCiScanAsync(CiScanRequest request);
    Task<bool> UpdateCiScanAsync(Guid scanId, UpdateCiScanRequest request);

    Task<UploadCiFindingResponse> UploadFinding(UploadCiFindingRequest request);
    Task<ScanDependencyResult> PushCiDependencyAsync(UploadCiDependencyRequest request);
}

public class CiService(
    IServiceProvider serviceProvider,
    AppDbContext context
) : ICiService
{
    public async Task<CiScanInfo> CreateCiScanAsync(CiScanRequest request)
    {
        return (await new CreateCiScanCommand(context)
            .ExecuteAsync(request)).GetResult();
    }

    public async Task<bool> UpdateCiScanAsync(Guid scanId, UpdateCiScanRequest request)
    {
        return (await new UpdateCiScanCommand(context)
            .ExecuteAsync(scanId, request)).GetResult();
    }

    public async Task<UploadCiFindingResponse> UploadFinding(UploadCiFindingRequest request)
    {
        return (await new PushCiFindingCommand(serviceProvider)
            .ExecuteAsync(request)).GetResult();
    }

    public Task<ScanDependencyResult> PushCiDependencyAsync(UploadCiDependencyRequest request)
    {
        return new PushCiDependencyCommand(context).ExecuteAsync(request);
    }
}