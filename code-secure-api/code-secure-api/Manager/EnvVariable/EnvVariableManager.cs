using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Exception;
using CodeSecure.Manager.EnvVariable.Model;
using Microsoft.EntityFrameworkCore;
using EnvironmentName = CodeSecure.Database.Entity.EnvironmentName;

namespace CodeSecure.Manager.EnvVariable;

public class EnvVariableManager(AppDbContext context): IEnvVariableManager
{
    public async Task<EnvironmentName?> FindByNameAsync(string name)
    {
        return await context.EnvironmentName.FirstOrDefaultAsync(record => record.Name == name);
    }

    public async Task<Page<string>> FilterAsync(QueryFilter filter)
    {
        return await context.EnvironmentName
            .Select(record => record.Name)
            .PageAsync(filter.Page, filter.Size);
    }

    public async Task CreateAsync(string name)
    {
        if (context.EnvironmentName.Any(record => record.Name == name))
        {
            throw new BadRequestException("env name exists");
        }
        context.EnvironmentName.Add(new EnvironmentName {Name = name});
        await context.SaveChangesAsync();
    } 
    
    public async Task RemoveAsync(string name)
    {
        var envName = await FindByNameAsync(name);
        if (envName != null)
        {
            context.EnvironmentName.Remove(envName);
            await context.SaveChangesAsync();
        }
    }

    public Task<Page<EnvironmentVariable>> GetProjectEnvironmentAsync(Guid projectId, QueryFilter filter)
    {
        return context.ProjectEnvironmentVariables
            .Where(record => record.ProjectId == projectId)
            .Select(record => new EnvironmentVariable
            {
                Name = record.Name,
                Value = record.Value
            })
            .PageAsync(filter.Page, filter.Size);
    }

    public async Task<EnvironmentVariable> SetProjectEnvironmentAsync(Guid projectId, string name, string value)
    {
        var project = await context.Projects.FirstOrDefaultAsync(record => record.Id == projectId);
        if (project == null)
        {
            throw new BadRequestException("project not found");
        }
        
        var envName = await FindByNameAsync(name);
        if (envName == null)
        {
            throw new BadRequestException("env not in whitelist");
        }

        var projectEnv = await context.ProjectEnvironmentVariables
            .FirstOrDefaultAsync(record => record.ProjectId == projectId && record.Name == name);
        if (projectEnv == null)
        {
            projectEnv = new ProjectEnvironmentVariables
            {
                ProjectId = projectId,
                Name = name,
                Value = value
            };
            context.ProjectEnvironmentVariables.Add(projectEnv);
        }
        else
        {
            projectEnv.Value = value;
            context.ProjectEnvironmentVariables.Update(projectEnv);
        }
        await context.SaveChangesAsync();
        return new EnvironmentVariable
        {
            Name = name,
            Value = value
        };
    }

    public async Task RemoveProjectEnvironmentAsync(Guid projectId, string name)
    {

        var envVariable = await context.ProjectEnvironmentVariables
            .FirstOrDefaultAsync(record => record.ProjectId == projectId && record.Name == name);
        if (envVariable != null)
        {
            context.ProjectEnvironmentVariables.Remove(envVariable);
            await context.SaveChangesAsync();
        }
    }
}