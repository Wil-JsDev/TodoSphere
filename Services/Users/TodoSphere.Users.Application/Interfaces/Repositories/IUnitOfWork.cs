namespace TodoSphere.Users.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }

    IRolesRepository Roles { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}