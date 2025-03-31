using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Scanner;

public static class ScannerExtension
{
    private static readonly Dictionary<Guid, Scanners> ScannersCache = new();

    public static async Task<Result<Scanners>> FindScannerByIdAsync(this AppDbContext context, Guid scannerId)
    {
        if (ScannersCache.TryGetValue(scannerId, out var scanner))
        {
            return scanner;
        }

        scanner = await context.Scanners.FirstOrDefaultAsync(record => record.Id == scannerId);
        if (scanner == null) return Result.Fail($"Scanner not found");
        ScannersCache[scannerId] = scanner;
        return scanner;
    }
    
    public static Task<List<Scanners>> ListScannersByTypeAsync(this AppDbContext context, ScannerType type)
    {
        return context.Scanners
            .Where(record => record.Type == type)
            .OrderByDescending(scanner => scanner.CreatedAt)
            .ToListAsync();
    }
    
    public static Task<List<Scanners>> ListScannersAsync(this AppDbContext context)
    {
        return context.Scanners
            .OrderByDescending(scanner => scanner.CreatedAt)
            .ToListAsync();
    }
    
    public static Task<List<Scanners>> ListScannersOfProjectAsync(this AppDbContext context, Guid projectId)
    {
        return context.Scans
            .Include(scan => scan.Scanner)
            .Where(scan => scan.ProjectId == projectId)
            .Select(scan => scan.Scanner!)
            .Distinct().ToListAsync();
    }
    

    public static async Task<Result<Scanners>> CreateScannerAsync(this AppDbContext context, Scanners request)
    {
        var scanner = await context.Scanners.FirstOrDefaultAsync(record =>
            record.NormalizedName == request.Name.NormalizeUpper()
            && record.Type == request.Type);
        if (scanner != null)
        {
            return scanner;
        }

        request.NormalizedName = request.Name.NormalizeUpper();
        request.Id = Guid.NewGuid();
        context.Scanners.Add(request);
        await context.SaveChangesAsync();
        return request;
    }
}