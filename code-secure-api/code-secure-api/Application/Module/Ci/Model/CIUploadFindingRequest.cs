using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Ci.Model;

public record CiUploadFindingRequest
{
    [Required] public required Guid ScanId { get; set; }

    public List<CiFinding> Findings { get; set; } = [];
}