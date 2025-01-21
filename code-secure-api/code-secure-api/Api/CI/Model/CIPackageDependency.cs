using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.CI.Model;

public record CiPackageDependency
{
    [Required] public required string PkgId { get; set; }

    // list PkgId of other Package
    public List<string>? Dependencies { get; set; }
}