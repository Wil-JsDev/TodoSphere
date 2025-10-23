using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.DTOs.Roles;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Application.Utils;
using TodoSphere.Users.Domain.Enum;

namespace TodoSphere.Users.Application.Interfaces.Services;

public interface IRolesService
{
    Task<ResultT<RolesDtOs>> CreateAsync(string name, CancellationToken cancellationToken = default);

    Task<ResultT<RolesDtOs>> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<ResultT<RolesDtOs>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ResultT<PagedResult<RolesDtOs>>> GetPagedRolesAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken = default);

    Task<ResultT<RolesDtOs>> UpdateAsync(Guid id, string name, CancellationToken cancellationToken = default);

    Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}