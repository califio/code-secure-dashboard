using CodeSecure.Core.Entity;

namespace CodeSecure.Application.Module.Scanner;

public static class ScannerFilterQueryable
{
    public static IQueryable<Scanners> ScannerFilter(this IQueryable<Scanners> query, AppDbContext context,
        ScannerFilter filter)
    {
        return query
            .Where(scanner => filter.Type == null || filter.Type.Count == 0 || filter.Type.Contains(scanner.Type))
            .Where(scanner => string.IsNullOrEmpty(filter.Name) || scanner.Name.Contains(filter.Name))
            .Where(scanner => filter.ProjectId == null || context.Scans.Any(scan =>
                scan.ProjectId == filter.ProjectId && scan.ScannerId == scanner.Id)
            );
    }
}