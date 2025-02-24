using System.Web;
using CodeSecure.Api.User.Model;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Manager.Integration.Mail;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Setting;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.User.Service;

public class DefaultUserService(
    AppDbContext context,
    IHttpContextAccessor contextAccessor,
    JwtUserManager userManager,
    IMailSender mailSender,
    ISettingManager settingManager
) : BaseService<Users>(contextAccessor), IUserService
{
    public async Task<Page<UserInfo>> GetUserInfoAsync(UserFilter filter)
    {
        var query = from user in context.Users
            join userRoles in context.UserRoles on user.Id equals userRoles.UserId
            join role in context.Roles on userRoles.RoleId equals role.Id
            select new
            {
                user, role
            };
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(record => record.user.UserName!.Contains(filter.Name));

        if (filter.RoleId != null) query = query.Where(record => record.role.Id == filter.RoleId);

        if (filter.Status != null) query = query.Where(record => record.user.Status == filter.Status);

        return await query.Select(record => new UserInfo
        {
            Id = record.user.Id,
            UserName = record.user.UserName!,
            FullName = record.user.FullName,
            Avatar = record.user.Avatar,
            Status = record.user.Status,
            Email = record.user.Email!,
            CreatedAt = record.user.CreatedAt,
            UpdatedAt = record.user.UpdatedAt,
            Role = record.role.Name!,
            Enable2Fa = record.user.TwoFactorEnabled,
            Verified = record.user.EmailConfirmed,
            Lockout = record.user.LockoutEnd
        }).OrderBy(filter.SortBy.ToString(), filter.Desc).PageAsync(filter.Page, filter.Size);
    }

    public async Task<List<UserSummary>> GetProjectManagerUsersAsync()
    {
        var users = await context.ProjectUsers
            .Include(record => record.User)
            .Where(record => record.Role == ProjectRole.Manager)
            .Select(record => record.User!).Distinct().ToListAsync();
        return users.Select(user => new UserSummary
        {
            Id = user.Id,
            UserName = user.UserName!,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Email = user.Email
        }).ToList();
    }

    public async Task<Page<UserSummary>> GetUserSummaryAsync(UserFilter filter)
    {
        var query = context.Users.Where(user => user.Status != UserStatus.Disabled && user.IsDefault == false);
        if (!string.IsNullOrEmpty(filter.Name)) query = query.Where(user => user.UserName!.Contains(filter.Name));

        if (filter.RoleId != null)
            query = query.Where(user =>
                context.UserRoles.Any(userRole => userRole.UserId == user.Id && userRole.RoleId == filter.RoleId));

        return await query.OrderBy(nameof(Users.CreatedAt), filter.Desc).Select(user => new UserSummary
        {
            Id = user.Id,
            UserName = user.UserName!,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Email = user.Email
        }).PageAsync(filter.Page, filter.Size);
    }

    public async Task<UserInfo> GetUserInfoByIdAsync(Guid userId)
    {
        var user = await FindByIdAsync(userId);
        if (!HasPermission(user, PermissionAction.Read)) throw new AccessDeniedException();

        var role = await context.Roles.FirstAsync(role => context.UserRoles.Any(record =>
            record.RoleId == role.Id && record.UserId == user.Id)
        );
        return new UserInfo
        {
            Status = user.Status,
            Email = user.Email!,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Role = role.Name!,
            Enable2Fa = user.TwoFactorEnabled,
            Id = user.Id,
            UserName = user.FullName,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Verified = user.EmailConfirmed,
            Lockout = user.LockoutEnd
        };
    }

    public async Task<UserInfo> CreateUserAsync(CreateUserRequest request)
    {
        var username = request.Email.Split('@')[0];
        var user = new Users
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = request.Email,
            EmailConfirmed = request.Verified,
            TwoFactorEnabled = false,
            FullName = username,
            Status = UserStatus.Active,
            Avatar = null,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, PasswordGenerator.GeneratePassword(32));
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, request.Role);
            SendConfirmEmail(user);
            return await GetUserInfoByIdAsync(user.Id);
        }

        throw new BadRequestException(result.Errors.First().Description);
    }

    public async Task<UserInfo> UpdateUserAsync(Guid userId, UpdateUserRequest request)
    {
        var user = await FindByIdAsync(userId);
        if (CurrentUser().Id == userId) throw new BadRequestException("you can't update your account");

        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            user.Email = request.Email;
            user.UserName = request.Email.Split('@')[0];
        }

        if (!string.IsNullOrEmpty(request.FullName) && request.FullName != user.FullName)
            user.FullName = request.FullName;
        if (request.Verified != null)
        {
            user.EmailConfirmed = (bool)request.Verified;
        }
        if (request.Status != null && request.Status != user.Status) user.Status = (UserStatus)request.Status;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new BadRequestException(result.Errors.First().Description);

        if (!string.IsNullOrEmpty(request.Role))
            if (await userManager.IsInRoleAsync(user, request.Role!) == false)
            {
                // delete all current role of user
                await context.UserRoles.Where(record => record.UserId == user.Id).ExecuteDeleteAsync();
                result = await userManager.AddToRoleAsync(user, request.Role!);
                if (!result.Succeeded) throw new BadRequestException(result.Errors.First().Description);
            }

        return await GetUserInfoByIdAsync(userId);
    }

    public async Task SendConfirmEmailAsync(Guid userId)
    {
        var user = await FindByIdAsync(userId);
        
        if (await userManager.IsEmailConfirmedAsync(user))
        {
            throw new BadRequestException("This account was confirmed");
        }
        SendConfirmEmail(user);
    }
    private void SendConfirmEmail(Users user)
    {
        var token = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
        token = HttpUtility.UrlEncode(token);
        var activeUrl = $"{FrontendUrl()}/#/auth/confirm-email?token={token}&username={user.UserName}";
        mailSender.SendInviteUser([user.Email!], new InviteUserModel
        {
            Username = user.UserName!,
            ConfirmUrl = activeUrl
        });
    }

    protected override bool HasPermission(Users entity, string action)
    {
        return CurrentUser().HasClaim(PermissionType.User, action);
    }

    protected override async Task<Users> FindByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Id == id)
               ?? throw new BadRequestException("User not found");
    }
    
}