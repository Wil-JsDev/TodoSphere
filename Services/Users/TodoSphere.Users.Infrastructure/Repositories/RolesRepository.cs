using Microsoft.EntityFrameworkCore;
using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.Interfaces.Repositories;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Domain.Models;
using TodoSphere.Users.Infrastructure.Context;
using TodoSphere.Users.Infrastructure.Repositories.Common;

namespace TodoSphere.Users.Infrastructure.Repositories;

public class RolesRepository(TodoSphereUserContext context) : Repository<Role>(context), IRolesRepository
{
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Roles.FirstOrDefaultAsync(rol => rol.Name == name, cancellationToken: cancellationToken);
    }

    public async Task<string?> GetRoleNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context
            .Roles
            .AsNoTracking()
            .Where(rol => rol.Name == name)
            .Select(rol => rol.Name)
            .FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(rol => rol.Name == name, cancellationToken: cancellationToken);
    }

    public async Task<PagedResult<Role>> GetPagedRolesAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken)
    {
        var baseQuery = Context.Roles
            .AsNoTracking()
            .OrderBy(rol => rol.RoleId);

        var total = await baseQuery.CountAsync(cancellationToken);

        var query = await baseQuery
            .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
            .Take(paginationParameter.PageSize)
            .ToListAsync(cancellationToken);

        var pagedResult =
            new PagedResult<Role>(query, total, paginationParameter.PageNumber, paginationParameter.PageSize);

        return pagedResult;
    }
}