using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.CI.Model;

public record CiUploadFindingRequest
{
    [Required] public required Guid ScanId { get; set; }

    public List<CiFinding> Findings { get; set; } = [];
}