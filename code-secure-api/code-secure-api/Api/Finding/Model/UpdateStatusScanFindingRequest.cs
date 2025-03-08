using System.ComponentModel.DataAnnotations;
using CodeSecure.Enum;

namespace CodeSecure.Api.Finding.Model;

public class UpdateStatusScanFindingRequest
{
    [Required]
    public required Guid ScanId { get; set; }
    [Required]
    public required FindingStatus Status { get; set; }
}