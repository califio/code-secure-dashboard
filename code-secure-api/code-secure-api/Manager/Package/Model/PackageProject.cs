using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Package.Model;

public record PackageProject
{
    public required Packages Package { get; set; }
    public required string Location { get; set; }
}