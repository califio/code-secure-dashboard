using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CodeSecure.Core.Entity;

public class BaseEntity
{
    [Key] public Guid Id { get; set; }
    [JsonIgnore]
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}