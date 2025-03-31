using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Core.Entity;

public class Roles : IdentityRole<Guid>
{
    public required bool IsDefault { get; set; }
}