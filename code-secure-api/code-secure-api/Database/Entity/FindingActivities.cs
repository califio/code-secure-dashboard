using CodeSecure.Database.Metadata;
using CodeSecure.Enum;
using CodeSecure.Extension;

namespace CodeSecure.Database.Entity;

public class FindingActivities : BaseEntity
{
    public Guid? UserId { get; init; }
    public Users? User { get; init; }
    public string? Comment { get; init; }
    public required FindingActivityType Type { get; init; }
    public required Guid FindingId { get; init; }
    public Findings? Finding { get; init; }

    public static FindingActivities ChangeDeadline(Guid? userId, Guid findingId, DateTime? previousDate,
        DateTime? currentDate)
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = FindingActivityType.ChangeDeadline,
            Metadata = JSONSerializer.Serialize(new FindingActivityMetadata
            {
                ChangeDeadline = new ChangeDeadlineFinding
                {
                    PreviousDate = previousDate,
                    CurrentDate = currentDate
                }
            }),
            FindingId = findingId,
            Comment = null
        };
    }

    public static FindingActivities ChangeStatus(Guid userId, Guid findingId, FindingStatus previousStatus,
        FindingStatus currentStatus)
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = FindingActivityType.ChangeStatus,
            Metadata = JSONSerializer.Serialize(new FindingActivityMetadata
            {
                ChangeStatus = new ChangeStatusFinding
                {
                    PreviousStatus = previousStatus,
                    CurrentStatus = currentStatus
                }
            }),
            FindingId = findingId,
            Comment = null
        };
    }

    public static FindingActivities AddComment(Guid userId, Guid findingId, string comment)
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = FindingActivityType.Comment,
            FindingId = findingId,
            Comment = comment
        };
    }
}