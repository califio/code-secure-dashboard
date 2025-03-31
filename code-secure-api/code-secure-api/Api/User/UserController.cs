using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.User;
using CodeSecure.Authentication;
using CodeSecure.Core.EntityFramework;
using FluentResults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.User;

public class UserController(
    IFindUserByIdHandler findUserByIdHandler,
    ICreateUserHandler createUserHandler,
    IUpdateUserHandler updateUserHandler,
    ISendConfirmEmailHandler sendConfirmEmailHandler,
    IQueryUserInfoHandler queryUserInfoHandler,
    IQueryUserSummaryHandler queryUserSummaryHandler,
    IListProjectManagerUserHandler listProjectManagerUserHandler
) : BaseController
{
    [HttpPost]
    [Route("public")]
    public async Task<Page<UserSummary>> QueryUserSummary(UserFilter filter)
    {
        var result = await queryUserSummaryHandler.HandleAsync(filter);
        return result.Value;
    }

    [HttpGet]
    [Route("project-manager")]
    public async Task<List<UserSummary>> ListProjectManagerUser()
    {
        var result = await listProjectManagerUserHandler.HandleAsync(true);
        return result.Value;
    }

    [HttpPost]
    [Route("filter")]
    [Permission(PermissionType.User, PermissionAction.Read)]
    public async Task<Page<UserInfo>> QueryUserInfo(UserFilter filter)
    {
        var result = await queryUserInfoHandler.HandleAsync(filter);
        return result.Value;
    }

    [HttpGet]
    [Route("{userId:guid}")]
    [Permission(PermissionType.User, PermissionAction.Read)]
    public async Task<UserInfo> FindUserInfoById(Guid userId)
    {
        var result = await findUserByIdHandler.HandleAsync(userId);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }

    [HttpPost]
    [Permission(PermissionType.User, PermissionAction.Create)]
    public async Task<UserInfo> CreateUser(CreateUserRequest request)
    {
        var result = await createUserHandler.HandleAsync(request)
            .Bind(user => findUserByIdHandler.HandleAsync(user.Id));
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }


    [HttpPut]
    [Route("{userId:guid}")]
    [Permission(PermissionType.User, PermissionAction.Update)]
    public async Task<UserInfo> UpdateUser(Guid userId, UpdateUserRequest request)
    {
        var result = await updateUserHandler.HandleAsync(userId, request)
            .Bind(user => findUserByIdHandler.HandleAsync(user.Id));
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }

    [HttpPost]
    [Route("{userId:guid}/send-confirm-email")]
    [Permission(PermissionType.User, PermissionAction.Update)]
    public async Task SendConfirmEmail(Guid userId)
    {
        var result = await sendConfirmEmailHandler.HandleAsync(userId);
        if (result.IsFailed)
        {
            throw new BadRequestException(result.Errors.Select(error => error.Message));
        }
    }
}