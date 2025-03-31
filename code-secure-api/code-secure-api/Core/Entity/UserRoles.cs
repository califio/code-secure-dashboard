using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
public class UserRoles : IdentityUserRole<Guid>;