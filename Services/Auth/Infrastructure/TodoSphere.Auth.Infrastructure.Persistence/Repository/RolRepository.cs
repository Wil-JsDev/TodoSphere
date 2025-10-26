using Microsoft.EntityFrameworkCore;
using TodoSphere.Auth.Application.Interfaces.Repositories;
using TodoSphere.Auth.Domain.Models;
using TodoSphere.Auth.Persistence.Context;

namespace TodoSphere.Auth.Persistence.Repository;

public class RolRepository(AuthContext context) : Repository<Roles>(context), IRolRepository
{
    public async Task<Roles?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(r => r.Name == name, cancellationToken: cancellationToken);
    }

    public async Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var rol = await GetByNameAsync(name, cancellationToken);

        Remove(rol!);

        await Context.SaveChangesAsync(cancellationToken);
    }
}