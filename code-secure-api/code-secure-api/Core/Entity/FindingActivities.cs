using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Core.Entity;

public class FindingActivities : BaseEntity
{
    public Guid? UserId { get; init; }
    public Users? User { get; init; }
    public string? Comment { get; init; }
    public required FindingActivityType Type { get; init; }
    public string? OldState { get; set; }
    public string? NewState { get; set; }
    public Guid? CommitId { get; set; }
    public GitCommits? Commit { get; set; }
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
            OldState = previousDate.ToString(),
            NewState = currentDate.ToString(),
            FindingId = findingId
        };
    }

    public static FindingActivities ChangeStatus(
        Guid userId,
        Guid findingId,
        FindingStatus oldStatus,
        FindingStatus newStatus,
        Guid? commitId = null
    )
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = FindingActivityType.ChangeStatus,
            OldState = oldStatus.ToString(),
            NewState = newStatus.ToString(),
            FindingId = findingId,
            CommitId = commitId
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

    public static FindingActivities OpenFinding(Guid findingId, Guid commitId)
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            Type = FindingActivityType.Open,
            CommitId = commitId,
            FindingId = findingId
        };
    }
    
    public static FindingActivities FixedFinding(Guid findingId, Guid commitId)
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            Type = FindingActivityType.Fixed,
            CommitId = commitId,
            FindingId = findingId
        };
    }

    public static FindingActivities ReopenFinding(Guid findingId, Guid commitId)
    {
        return new FindingActivities
        {
            Id = Guid.NewGuid(),
            Type = FindingActivityType.Reopen,
            CommitId = commitId,
            FindingId = findingId
        };
    }
}