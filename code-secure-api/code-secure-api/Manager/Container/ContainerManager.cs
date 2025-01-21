using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Container;

public class ContainerManager(AppDbContext context): IContainerManager
{
    public async Task<Containers> CreateAsync(Containers container)
    {
        var containers = await context.Containers.FirstOrDefaultAsync(record =>
            record.NormalizedName == container.Name.NormalizeUpper());
        if (containers != null)
        {
            return containers;
        }
        container.NormalizedName = container.Name.NormalizeUpper();
        container.Id = Guid.NewGuid();
        context.Containers.Add(container);
        await context.SaveChangesAsync();
        return container;
    }
    
}