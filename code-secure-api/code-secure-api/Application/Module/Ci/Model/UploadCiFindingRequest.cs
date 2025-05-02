using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Ci.Model;

public record UploadCiFindingRequest
{
    [Required] public required Guid ScanId { get; set; }

    public List<CiFinding> Findings { get; set; } = [];
    public ScanStrategy? Strategy { get; set; } = ScanStrategy.AllFiles;
    public List<ChangedFile> ChangedFiles { get; set; } = [];
}