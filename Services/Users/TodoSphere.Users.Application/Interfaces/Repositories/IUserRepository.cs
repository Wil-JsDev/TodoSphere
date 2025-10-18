using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.Interfaces.Common;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<PagedResult<User>> GetPagedUsersAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken = default);
}