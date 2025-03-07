using CodeSecure.Database.Entity;
using CodeSecure.Manager.Scanner;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Scanner;

public class ScannerController(IScannerManager scannerManager) : BaseController
{
    [HttpGet]
    public Task<List<Scanners>> GetScanners(Guid? projectId)
    {
        return scannerManager.GetScannersAsync(projectId);
    }

    [HttpGet]
    [Route("sast")]
    public Task<List<Scanners>> GetSastScanners(Guid? projectId)
    {
        return scannerManager.GetSastScannersAsync(projectId);
    }

    [HttpGet]
    [Route("sca")]
    public Task<List<Scanners>> GetScaScanners(Guid? projectId)
    {
        return scannerManager.GetScaScannersAsync(projectId);
    }
}