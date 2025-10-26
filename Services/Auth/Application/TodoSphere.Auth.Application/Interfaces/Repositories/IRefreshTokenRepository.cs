using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<bool> ExistsByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<bool> DeleteByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<bool> IsExpiredAsync(string token, CancellationToken cancellationToken = default);

    Task<bool> IsRevokedAsync(string token, CancellationToken cancellationToken = default);
}