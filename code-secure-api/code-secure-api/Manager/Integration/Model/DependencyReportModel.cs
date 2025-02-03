using CodeSecure.Manager.Project.Model;

namespace CodeSecure.Manager.Integration.Model;

public record DependencyReportModel
{
    public required string RepoUrl { get; set; }
    public required string RepoName { get; set; }
    public required string ProjectDependencyUrl { get; set; }
    public required int Critical { get; set; }
    public required int High { get; set; }
    public required int Medium { get; set; }
    public required int Low { get; set; }
    public required List<DependencyProject> Packages { get; set; }
}