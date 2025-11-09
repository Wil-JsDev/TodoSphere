using TodoSphere.Auth.Application.DTOs.Auth;
using TodoSphere.Auth.Application.Utils;
using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user);

    Task<RefreshToken> GenerateRefreshToken(User user, CancellationToken cancellationToken = default);

    Task<ResultT<AuthenticationResponse>>
        RefreshTokenAsync(string refreshToken);
}