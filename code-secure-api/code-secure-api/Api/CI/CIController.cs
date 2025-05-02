using CodeSecure.Application.Module.Ci;
using CodeSecure.Application.Module.Ci.Model;
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
        return await ciService.CreateCiScanAsync(request);
    }

    [HttpPut]
    [Route("scan/{scanId:guid}")]
    public async Task UpdateCiScan(Guid scanId, UpdateCiScanRequest request)
    {
        await ciService.UpdateCiScanAsync(scanId, request);
    }

    [HttpPost]
    [Route("finding")]
    public async Task<UploadCiFindingResponse> UploadCiFinding(UploadCiFindingRequest request)
    {
        return await ciService.UploadFinding(request);
    }

    [HttpPost]
    [Route("dependency")]
    public async Task<ScanDependencyResult> UploadCiDependency(UploadCiDependencyRequest request)
    {
        return await ciService.PushCiDependencyAsync(request);
    }
}