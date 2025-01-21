using CodeSecure.Database.Entity;

namespace CodeSecure.Manager.Container;

public interface IContainerManager
{
    Task<Containers> CreateAsync(Containers container);
}