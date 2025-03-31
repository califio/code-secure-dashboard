using CodeSecure.Api.CI.Model;
namespace CodeSecure.Api.CI.Service;

public interface ICiService
{
    Task<CiScanInfo> InitScan(CiScanRequest request);
    Task UpdateScan(Guid scanId, UpdateCiScanRequest request);

    Task<CiUploadFindingResponse> UploadFinding(CiUploadFindingRequest request);
    Task<ScanDependencyResult> UploadDependency(CiUploadDependencyRequest request);
}