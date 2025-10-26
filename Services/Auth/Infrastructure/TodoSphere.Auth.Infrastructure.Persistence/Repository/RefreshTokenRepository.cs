using Microsoft.EntityFrameworkCore;
using TodoSphere.Auth.Application.Interfaces.Repositories;
using TodoSphere.Auth.Domain.Models;
using TodoSphere.Auth.Persistence.Context;

namespace TodoSphere.Auth.Persistence.Repository;

public class RefreshTokenRepository(AuthContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await Context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<bool> ExistsByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<bool> DeleteByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetByTokenAsync(token, cancellationToken);
        if (refreshToken == null)
        {
            return false;
        }

        Context.RefreshTokens.Remove(refreshToken);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> IsExpiredAsync(string token, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(x => x.Token == token && x.ExpiresAt <= DateTime.UtcNow, cancellationToken);
    }

    public async Task<bool> IsRevokedAsync(string token, CancellationToken cancellationToken = default)
    {
        return await ValidateAsync(x => x.Token == token && x.RevokedAt != null, cancellationToken);
    }
}