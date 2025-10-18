using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.DTOs.Users;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Application.Utils;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Interfaces.Services;

public interface IUserService
{
    Task<ResultT<UserDtOs>> CreateAsync(CreateUserDtOs createUser, CancellationToken cancellationToken = default);

    Task<ResultT<PagedResult<UserDtOs>>> GetPagedUsersAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken = default);

    Task<Result> AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);

    Task<Result> RemoveRoleFromUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
}