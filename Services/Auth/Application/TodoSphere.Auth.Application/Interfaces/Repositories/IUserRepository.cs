using TodoSphere.Auth.Application.Utils;
using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<PagedResult<User>> GetPagedUserAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default);
}