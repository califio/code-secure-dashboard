using CodeSecure.Application.Module.Scanner.Command;
using CodeSecure.Application.Module.Scanner.Model;
using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Scanner;

public static class ScannerExtension
{
    private static readonly Dictionary<Guid, Scanners> ScannersCache = new();
    public static async Task<Result<Scanners>> GetScannerByIdAsync(this AppDbContext context, Guid scannerId)
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

    public static Task<Result<Scanners>> CreateScannerAsync(this AppDbContext context, CreateScannerRequest request)
    {
        return new CreateScannerCommand(context).ExecuteAsync(request);
    }
}