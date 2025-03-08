using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public class UpdateProjectPackageRequest
{
    public PackageStatus? Status { get; set; }
    public string? IgnoreReason { get; set; }
}