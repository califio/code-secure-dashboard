using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Database;

public class BaseEntity
{
    [Key] public Guid Id { get; set; }

    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}