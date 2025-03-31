using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(Name), IsUnique = true)]
public class Tags : BaseEntity
{
    public required string Name { get; set; }
    public required TagType Type { get; set; }
}