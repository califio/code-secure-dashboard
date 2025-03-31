using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[PrimaryKey(nameof(ProjectId), nameof(TagId))]
public class ProjectTags
{
    public required Guid ProjectId;
    public required Guid TagId;
    //
    public Projects? Project;
    public Tags? Tag;
}