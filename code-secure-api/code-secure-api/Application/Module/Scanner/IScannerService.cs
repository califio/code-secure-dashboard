using CodeSecure.Application.Module.Scanner.Model;
using CodeSecure.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Scanner;

public interface IScannerService
{
    Task<List<Scanners>> ListScannersAsync(ScannerFilter filter);
}

public class ScannerService(AppDbContext context) : IScannerService
{
    public Task<List<Scanners>> ListScannersAsync(ScannerFilter filter)
    {
        return context.Scanners
            .ScannerFilter(context, filter)
            .Distinct()
            .ToListAsync();
    }
}