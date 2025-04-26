using CodeSecure.Application.Module.User;
using CodeSecure.Application.Module.User.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.User;

public class UserController(
    IUserService userService
) : BaseController
{
    [HttpPost]
    [Route("public")]
    public Task<Page<UserSummary>> GetUserSummaryByFilter(UserFilter filter)
    {
        return userService.GetUserSummaryByFilterAsync(filter);
    }

    [HttpGet]
    [Route("project-manager")]
    public Task<List<UserSummary>> ListProjectManagerUser()
    {
        return userService.ListProjectManagerUserAsync();
    }

    [HttpPost]
    [Route("filter")]
    [Permission(PermissionType.User, PermissionAction.Read)]
    public Task<Page<UserDetail>> GetUserDetailByFilter(UserFilter filter)
    {
        return userService.GetUserDetailByFilterAsync(filter);
    }

    [HttpGet]
    [Route("{userId:guid}")]
    [Permission(PermissionType.User, PermissionAction.Read)]
    public Task<UserDetail> GetUserById(Guid userId)
    {
        return userService.GetUserByIdAsync(userId);
    }

    [HttpPost]
    [Permission(PermissionType.User, PermissionAction.Create)]
    public Task<UserDetail> CreateUser(CreateUserRequest request)
    {
        return userService.CreateUserAsync(request);
    }

    [HttpPut]
    [Route("{userId:guid}")]
    [Permission(PermissionType.User, PermissionAction.Update)]
    public Task<UserDetail> UpdateUser(Guid userId, UpdateUserRequest request)
    {
        return userService.UpdateUserAsync(userId, request);
    }

    [HttpPost]
    [Route("{userId:guid}/send-confirm-email")]
    [Permission(PermissionType.User, PermissionAction.Update)]
    public Task SendConfirmEmail(Guid userId)
    {
        return userService.SendConfirmEmailAsync(userId);
    }
}