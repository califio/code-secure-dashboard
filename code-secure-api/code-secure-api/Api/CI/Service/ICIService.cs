using CodeSecure.Api.CI.Model;
using CodeSecure.Manager.EnvVariable.Model;

namespace CodeSecure.Api.CI.Service;

public interface ICiService
{
    Task<CiScanInfo> InitScan(CiScanRequest request);
    Task<List<EnvironmentVariable>> GetEnvironmentVariables(Guid scanId);
    Task UpdateScan(Guid scanId, UpdateCiScanRequest request);

    Task<CiUploadFindingResponse> UploadFinding(CiUploadFindingRequest request);
    Task<CiUploadDependencyResponse> UploadDependency(CiUploadDependencyRequest request);
}