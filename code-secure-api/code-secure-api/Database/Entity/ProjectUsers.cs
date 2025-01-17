using CodeSecure.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ProjectId), nameof(UserId))]
public class ProjectUsers
{
    public required Guid UserId { get; set; }
    public required Guid ProjectId { get; set; }
    public required ProjectRole Role { get; set; }
    public required Guid AddedById { get; set; }
    public required bool ReceiveNotification { get; set; }
    public required DateTime CreatedAt { get; set; }

    public Users? User { get; set; }
    public Projects? Project { get; set; }
    public Users? AddedBy { get; set; }
}