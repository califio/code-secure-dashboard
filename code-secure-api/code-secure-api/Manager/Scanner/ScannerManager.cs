using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CodeSecure.Manager.Scanner;

public class ScannerManager(
    AppDbContext context,
    IMemoryCache cache
) : IScannerManager
{
    private static readonly SemaphoreSlim Lock = new(1, 1);
    private const int ExpiredTime = 5;

    private static readonly List<ScannerType> ScaTypes =
    [
        ScannerType.Dependency,
        ScannerType.Container
    ];

    private static readonly List<ScannerType> SastTypes =
    [
        ScannerType.Sast,
        ScannerType.Secret
    ];

    public async Task<Scanners?> FindByIdAsync(Guid scannerId)
    {
        if (cache.TryGetValue(CacheKey(scannerId), out Scanners? scanner))
        {
            return scanner;
        }

        scanner = await context.Scanners.FirstOrDefaultAsync(record => record.Id == scannerId);
        CacheScanner(CacheKey(scannerId), scanner);
        return scanner;
    }

    public async Task<Scanners> CreateOrUpdateAsync(Scanners scanner)
    {
        await Lock.WaitAsync();
        try
        {
            var scanners = await context.Scanners.FirstOrDefaultAsync(record =>
                record.NormalizedName == scanner.Name.NormalizeUpper()
                && record.Type == scanner.Type);
            if (scanners != null)
            {
                return scanners;
            }

            scanner.NormalizedName = scanner.Name.NormalizeUpper();
            scanner.Id = Guid.NewGuid();
            context.Scanners.Add(scanner);
            await context.SaveChangesAsync();
            return scanner;
        }
        finally
        {
            Lock.Release();
        }
    }

    public async Task<List<Scanners>> GetScannersAsync(Guid? projectId = null)
    {
        if (projectId != null)
        {
            return await context.Scans
                .Include(scan => scan.Scanner)
                .Where(scan => scan.ProjectId == projectId)
                .Select(scan => scan.Scanner!)
                .Distinct().ToListAsync();
        }

        return await context.Scanners
            .ToListAsync();
    }

    public async Task<List<Scanners>> GetSastScannersAsync()
    {
        return await context.Scanners
            .Where(scanner => SastTypes.Contains(scanner.Type))
            .ToListAsync();
    }

    public async Task<List<Scanners>> GetScaScannersAsync()
    {
        return await context.Scanners
            .Where(scanner => ScaTypes.Contains(scanner.Type))
            .ToListAsync();
    }

    public Task<List<Scanners>> GetScannerByTypeAsync(ScannerType type)
    {
        return context.Scanners.Where(record => record.Type == type).ToListAsync();
    }

    public async Task<bool> IsScaScanner(Guid scannerId)
    {
        var scanner = await FindByIdAsync(scannerId);
        if (scanner == null)
        {
            return false;
        }

        return IsScaScanner(scanner.Type);
    }

    public bool IsScaScanner(ScannerType scannerType)
    {
        return ScaTypes.Contains(scannerType);
    }

    public async Task<bool> IsSastScanner(Guid scannerId)
    {
        var scanner = await FindByIdAsync(scannerId);
        if (scanner == null)
        {
            return false;
        }

        return IsSastScanner(scanner.Type);
    }

    public bool IsSastScanner(ScannerType scannerType)
    {
        return SastTypes.Contains(scannerType);
    }

    public bool IsScannerType(Guid scannerId, ScannerType scannerType)
    {
        var scanner = FindByIdAsync(scannerId).Result;
        if (scanner == null)
        {
            return false;
        }

        return IsScannerType(scanner, scannerType);
    }

    public bool IsScannerType(Scanners scanner, ScannerType scannerType)
    {
        return scanner.Type == scannerType;
    }

    private void CacheScanner(string key, Scanners? scanner)
    {
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(ExpiredTime));
        cache.Set(key, scanner, options);
    }

    private string CacheKey(Guid id)
    {
        return $"scanner_id:{id.ToString()}";
    }
}