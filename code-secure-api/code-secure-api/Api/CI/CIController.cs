using CodeSecure.Api.CI.Model;
using CodeSecure.Api.CI.Service;
using CodeSecure.Manager.EnvVariable.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.CI;

[ApiController]
[Route("api/ci")]
[AllowAnonymous]
[CiAuthorize]
[Produces("application/json")]
[Consumes("application/json")]
public class CiController(ICiService ciService)
{
    [HttpGet]
    [Route("ping")]
    public string Ping()
    {
        return "pong";
    }

    [HttpPost]
    [Route("scan")]
    public async Task<CiScanInfo> InitCiScan(CiScanRequest request)
    {
        return await ciService.InitScan(request);
    }

    [HttpGet]
    [Route("scan/{scanId:guid}/env")]
    public async Task<List<EnvironmentVariable>> GetScanEnvironmentVariables(Guid scanId)
    {
        return await ciService.GetEnvironmentVariables(scanId);
    }

    [HttpPut]
    [Route("scan/{scanId:guid}")]
    public async Task UpdateCiScan(Guid scanId, UpdateCiScanRequest request)
    {
        await ciService.UpdateScan(scanId, request);
    }

    [HttpPost]
    [Route("finding")]
    public async Task<CiUploadFindingResponse> UploadCiFinding(CiUploadFindingRequest request)
    {
        return await ciService.UploadFinding(request);
    }

    [HttpPost]
    [Route("dependency")]
    public async Task<ScanDependencyResult> UploadCiDependency(CiUploadDependencyRequest request)
    {
        return await ciService.UploadDependency(request);
    }
}