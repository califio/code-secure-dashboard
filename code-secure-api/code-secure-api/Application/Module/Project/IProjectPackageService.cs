using CodeSecure.Application.Module.Project.Command;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Project;

public interface IProjectPackageService
{
    Task<Tickets> CreateProjectPackageTicketAsync(CreateProjectPackageTicketRequest request);
    Task<bool> DeleteProjectPackageTicketAsync(Guid projectId, Guid packageId);
    Task<Page<ProjectPackage>> GetProjectPackageByFilterAsync(Guid projectId, ProjectPackageFilter filter);
    Task<ProjectPackageDetailResponse> GetProjectPackageDetailAsync(Guid projectId, Guid packageId);

    Task<ProjectPackageDetailResponse> UpdateProjectPackageAsync(Guid projectId, Guid packageId, UpdateProjectPackageRequest request);
}

public class ProjectPackageService(AppDbContext context) : IProjectPackageService
{
    public async Task<Tickets> CreateProjectPackageTicketAsync(CreateProjectPackageTicketRequest request)
    {
        return (await new CreateProjectPackageTicketCommand(context)
            .ExecuteAsync(request)).GetResult();
    }

    public async Task<bool> DeleteProjectPackageTicketAsync(Guid projectId, Guid packageId)
    {
        return (await new DeleteProjectPackageTicketCommand(context)
            .ExecuteAsync(projectId, packageId)).GetResult();
    }

    public async Task<Page<ProjectPackage>> GetProjectPackageByFilterAsync(Guid projectId, ProjectPackageFilter filter)
    {
        return (await new GetProjectPackageByFilterCommand(context)
            .ExecuteAsync(projectId, filter)).GetResult();
    }

    public async Task<ProjectPackageDetailResponse> GetProjectPackageDetailAsync(Guid projectId, Guid packageId)
    {
        return (await new GetProjectPackageDetailCommand(context)
            .ExecuteAsync(projectId, packageId)).GetResult();
    }

    public async Task<ProjectPackageDetailResponse> UpdateProjectPackageAsync(Guid projectId, Guid packageId, UpdateProjectPackageRequest request)
    {
        (await new UpdateProjectPackageCommand(context)
            .ExecuteAsync(projectId, packageId, request)).GetResult();
        return await GetProjectPackageDetailAsync(projectId, packageId);
    }
}