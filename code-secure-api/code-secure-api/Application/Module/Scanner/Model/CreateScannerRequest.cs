using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Scanner.Model;

public record CreateScannerRequest
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required ScannerType Type { get; set; }
}