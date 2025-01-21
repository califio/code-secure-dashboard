namespace CodeSecure.Manager.Project.Model;

public class DependencyProject
{
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required string Recommendation { get; set; }
    public required string Impact { get; set; }
}