using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Interfaces.Repositories;

/// <summary>
/// Defines the repository operations for RefreshToken entities.
/// </summary>
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    /// <summary>
    /// <param name="token">The opaque refresh token string to search for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the <see cref="RefreshToken"/> if found; otherwise, null.
    /// </returns>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all non-revoked and non-expired refresh tokens for a specific user.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a list of <see cref="RefreshToken"/> entities.
    /// </returns>
    Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a refresh token by its opaque token string.
    /// </summary>
    /// <param name="token">The opaque refresh token string to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result is true if the token was successfully deleted; otherwise, false.
    /// </returns>
    Task<bool> DeleteByTokenAsync(string token, CancellationToken cancellationToken = default);
}