using AutoMapper;
using CodeSecure.Api.Profile.Model;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Exception;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Profile.Service;

public class DefaultProfileService(
    IMapper mapper,
    AppDbContext context,
    IHttpContextAccessor contextAccessor
) : BaseService<Users>(contextAccessor), IProfileService
{
    public async Task<UserProfile> GetProfileAsync()
    {
        var currentUser = CurrentUser();
        var user = await FindByIdAsync(currentUser.Id);
        return mapper.Map<UserProfile>(user);
    }

    public Task<UserProfile> UpdateProfileAsync(UpdateProfileRequest request)
    {
        throw new NotImplementedException();
    }

    protected override bool HasPermission(Users entity, string action)
    {
        throw new NotImplementedException();
    }

    protected override async Task<Users> FindByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Id == id)
               ?? throw new BadRequestException("user not found");
    }
}