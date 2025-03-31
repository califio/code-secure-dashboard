using CodeSecure.Core.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;
[Index(nameof(Name), IsUnique = true)]
public class CiTokens : BaseEntity
{
    public required string Name { get; set; }
    public required string Value { get; set; }
    public DateTime? ExpiredAt { get; set; }
}