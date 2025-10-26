using TodoSphere.Auth.Application.Interfaces.Repositories;
using TodoSphere.Auth.Application.Interfaces.UnitOfWork;
using TodoSphere.Auth.Persistence.Context;
using TodoSphere.Auth.Persistence.Repository;

namespace TodoSphere.Auth.Persistence.UnitOfWork;

public class UnitOfWork(AuthContext authContext) : IUnitOfWork
{
    public IUserRepository Users { get; private set; } = new UserRepository(authContext);

    public IRefreshTokenRepository RefreshTokens { get; private set; } = new RefreshTokenRepository(authContext);

    public IRolRepository Roles { get; private set; } = new RolRepository(authContext);

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default) =>
        await authContext.SaveChangesAsync(cancellationToken);
}