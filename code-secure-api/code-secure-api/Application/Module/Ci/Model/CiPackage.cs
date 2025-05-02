namespace CodeSecure.Application.Module.Ci.Model;

public record CiPackage
{
    public Guid? Id { get; set; }

    // package id. look like pkg:maven/org.springframework/spring-webmvc@4.1.6.RELEASE
    public required string PkgId { get; set; }
    public string? Group { get; set; }
    public required string Name { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; }

    public string? License { get; set; }

    /*
     * if location != null (pom.xml, go.mod,...) -> this package is dependency of project
     */
    public string? Location { get; set; }
}