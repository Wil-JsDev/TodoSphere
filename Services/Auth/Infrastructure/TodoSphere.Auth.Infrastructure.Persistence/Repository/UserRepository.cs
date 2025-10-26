using Microsoft.EntityFrameworkCore;
using TodoSphere.Auth.Application.Interfaces.Repositories;
using TodoSphere.Auth.Application.Utils;
using TodoSphere.Auth.Domain.Models;
using TodoSphere.Auth.Persistence.Context;

namespace TodoSphere.Auth.Persistence.Repository;

public class UserRepository(AuthContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<PagedResult<User>> GetPagedUserAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = Context.Users.AsNoTracking();

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var query = baseQuery
            .OrderBy(us => us.UserId)
            .Include(us => us.Role)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var users = await query.ToListAsync(cancellationToken);

        return new PagedResult<User>(users, pageNumber, pageSize, totalCount);
    }
}