using CodeSecure.Application.Module.Scanner;
using CodeSecure.Application.Module.Scanner.Model;
using CodeSecure.Core.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Scanner;

public class ScannerController(
    IScannerService scannerService
) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public Task<List<Scanners>> GetScanners(ScannerFilter filter)
    {
        return scannerService.ListScannersAsync(filter);
    }
}