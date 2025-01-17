using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Database.Entity;

public class Roles : IdentityRole<Guid>
{
    public required bool IsDefault { get; set; }
}