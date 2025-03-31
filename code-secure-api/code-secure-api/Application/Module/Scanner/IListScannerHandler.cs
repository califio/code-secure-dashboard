using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Scanner;

public interface IListScannerHandler : IHandler<ScannerFilter, List<Scanners>>;
public class ListScannerHandler(AppDbContext context): IListScannerHandler
{
    public async Task<Result<List<Scanners>>> HandleAsync(ScannerFilter request)
    {
        return await context.Scanners
            .ScannerFilter(context, request)
            .Distinct()
            .ToListAsync();

    }
}