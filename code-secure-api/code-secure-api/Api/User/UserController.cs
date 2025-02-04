using CodeSecure.Api.User.Model;
using CodeSecure.Api.User.Service;
using CodeSecure.Authentication;
using CodeSecure.Database.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.User;

public class UserController(IUserService userService) : BaseController
{
    [HttpPost]
    [Route("public")]
    public async Task<Page<UserSummary>> GetUsers(UserFilter filter)
    {
        return await userService.GetUserSummaryAsync(filter);
    }

    [HttpPost]
    [Route("filter")]
    [Permission(PermissionType.User, PermissionAction.Read)]
    public async Task<Page<UserInfo>> GetUsersByAdmin(UserFilter filter)
    {
        return await userService.GetUserInfoAsync(filter);
    }

    [HttpGet]
    [Route("{userId:guid}")]
    [Permission(PermissionType.User, PermissionAction.Read)]
    public async Task<UserInfo> GetUser(Guid userId)
    {
        return await userService.GetUserInfoByIdAsync(userId);
    }

    [HttpPost]
    [Permission(PermissionType.User, PermissionAction.Create)]
    public async Task<UserInfo> CreateUserByAdmin(CreateUserRequest request)
    {
        return await userService.CreateUserAsync(request);
    }


    [HttpPut]
    [Route("{userId:guid}")]
    [Permission(PermissionType.User, PermissionAction.Update)]
    public async Task<UserInfo> UpdateUserByAdmin(Guid userId, UpdateUserRequest request)
    {
        return await userService.UpdateUserAsync(userId, request);
    }
    
    [HttpPost]
    [Route("{userId:guid}/send-confirm-email")]
    [Permission(PermissionType.User, PermissionAction.Update)]
    public async Task SendConfirmEmail(Guid userId)
    {
        await userService.SendConfirmEmailAsync(userId);
    }
}