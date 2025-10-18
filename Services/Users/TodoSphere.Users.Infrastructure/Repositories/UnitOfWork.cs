using TodoSphere.Users.Application.Interfaces.Repositories;
using TodoSphere.Users.Infrastructure.Context;

namespace TodoSphere.Users.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TodoSphereUserContext _context;
    public IUserRepository Users { get; private set; }
    public IRolesRepository Roles { get; private set; }

    public UnitOfWork(TodoSphereUserContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Roles = new RolesRepository(_context);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}