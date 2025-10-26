using TodoSphere.Auth.Application.Interfaces.Repositories;

namespace TodoSphere.Auth.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IRefreshTokenRepository RefreshTokens { get; }

    IRolRepository Roles { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}