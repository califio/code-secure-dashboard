using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Ci.Model;

public record CiUploadDependencyRequest
{
    [Required] 
    public required Guid ScanId { get; set; }

    public List<CiPackage>? Packages { get; set; }

    public List<CiPackageDependency>? PackageDependencies { get; set; }
    public List<CiVulnerability>? Vulnerabilities { get; set; }
}