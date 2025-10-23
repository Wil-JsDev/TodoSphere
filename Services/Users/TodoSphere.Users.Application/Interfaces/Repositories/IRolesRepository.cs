using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.Interfaces.Common;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Interfaces.Repositories;

/// <summary>
/// Represents a repository interface specifically designed to manage and query roles within the system.
/// Inherits the generic repository functionalities from <see cref="IRepository{TEntity}"/> for the <see cref="Role"/> entity.
/// Provides additional features tailored for roles, such as retrieval by name, existence checks, and paginated queries.
/// </summary>
public interface IRolesRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<string?> GetRoleNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<PagedResult<Role>> GetPagedRolesAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken);
}