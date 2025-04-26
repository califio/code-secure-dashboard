using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public class UpdateProjectPackageRequest
{
    public PackageStatus? Status { get; set; }
    public string? IgnoreReason { get; set; }
}