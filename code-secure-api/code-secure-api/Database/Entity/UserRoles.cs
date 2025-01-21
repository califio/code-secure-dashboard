using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
public class UserRoles : IdentityUserRole<Guid>;