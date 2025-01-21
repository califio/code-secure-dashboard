using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Database.Entity;

public class UserTokens : IdentityUserToken<Guid>
{
    [Key] public required Guid Id { get; set; }

    public required DateTime? ExpiredAt { get; set; }
}