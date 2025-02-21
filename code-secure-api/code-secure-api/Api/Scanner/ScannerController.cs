using CodeSecure.Database.Entity;
using CodeSecure.Manager.Scanner;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Scanner;

public class ScannerController(IScannerManager scannerManager): BaseController
{
    [HttpGet]
    public Task<List<Scanners>> GetScanners(Guid? projectId)
    {
        return scannerManager.GetScannersAsync(projectId);
    }

}