using CodeSecure.Application.Module.Scanner;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Scanner;

public class ScannerController(
    IListScannerHandler listScannerHandler
) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public async Task<List<Scanners>> GetScanners(ScannerFilter filter)
    {
        var result = await listScannerHandler.HandleAsync(filter);
        return result.GetResult();
    }
}