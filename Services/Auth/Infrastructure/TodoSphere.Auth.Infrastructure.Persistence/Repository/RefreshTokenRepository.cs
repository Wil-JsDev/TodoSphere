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

    public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.RefreshTokens
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetByTokenAsync(token, cancellationToken);
        if (refreshToken is null)
        {
            return false;
        }

        Remove(refreshToken);
        return true;
    }
}