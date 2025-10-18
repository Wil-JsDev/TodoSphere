using Microsoft.EntityFrameworkCore;
using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.Interfaces.Repositories;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Domain.Models;
using TodoSphere.Users.Infrastructure.Context;
using TodoSphere.Users.Infrastructure.Repositories.Common;

namespace TodoSphere.Users.Infrastructure.Repositories;

public class UserRepository(TodoSphereUserContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Users.FirstOrDefaultAsync(us => us.Email == email, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(us => us.Email == email, cancellationToken: cancellationToken);
    }

    public async Task<PagedResult<User>> GetPagedUsersAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = Context
            .Users
            .AsNoTracking()
            .OrderBy(us => us.UserId);

        var total = await baseQuery.CountAsync(cancellationToken);

        var query = await baseQuery
            .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
            .Take(paginationParameter.PageSize)
            .ToListAsync(cancellationToken);

        var pagedResult =
            new PagedResult<User>(query, total, paginationParameter.PageNumber, paginationParameter.PageSize);

        return pagedResult;
    }
}