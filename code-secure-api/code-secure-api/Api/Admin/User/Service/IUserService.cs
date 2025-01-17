using CodeSecure.Api.Admin.User.Model;
using CodeSecure.Database.Extension;

namespace CodeSecure.Api.Admin.User.Service;

public interface IUserService
{
    public Task<Page<UserSummary>> GetUserSummaryAsync(UserFilter filter);
    public Task<Page<UserInfo>> GetUserInfoAsync(UserFilter filter);
    public Task<UserInfo> GetUserInfoByIdAsync(Guid userId);
    public Task<UserInfo> CreateUserAsync(CreateUserRequest request);
    public Task<UserInfo> UpdateUserAsync(Guid userId, UpdateUserRequest request);
    public Task SendConfirmEmailAsync(Guid userId);
}