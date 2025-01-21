using CodeSecure.Database.Entity;
using CodeSecure.Enum;

namespace CodeSecure.Manager.Scanner;

public interface IScannerManager
{
    Task<Scanners?> FindByIdAsync(Guid scannerId);
    Task<Scanners> CreateOrUpdateAsync(Scanners scanner);
    Task<List<Scanners>> GetSastScannersAsync();
    Task<List<Scanners>> GetScaScannersAsync();
    Task<List<Scanners>> GetScannerByTypeAsync(ScannerType type);
    Task<bool> IsScaScanner(Guid scannerId);
    bool IsScaScanner(ScannerType scannerType);
    Task<bool> IsSastScanner(Guid scannerId);
    bool IsSastScanner(ScannerType scannerType);
    bool IsScannerType(Guid scannerId, ScannerType scannerType);
    bool IsScannerType(Scanners scanner, ScannerType scannerType);
}