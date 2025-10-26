using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Interfaces.Repositories;

public interface IRolRepository : IRepository<Roles>
{
    Task<Roles?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}