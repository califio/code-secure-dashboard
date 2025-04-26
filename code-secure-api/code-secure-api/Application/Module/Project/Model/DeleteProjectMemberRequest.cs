namespace CodeSecure.Application.Module.Project.Model;

public record DeleteProjectMemberRequest
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
}