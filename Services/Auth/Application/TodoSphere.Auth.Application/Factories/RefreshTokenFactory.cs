using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Factories;

public static class RefreshTokenFactory
{
    public static RefreshToken Create(string token, Guid userId, DateTime expiresAt) => new RefreshToken
    {
        RefreshTokenId = Guid.NewGuid(),
        Token = token,
        UserId = userId,
        ExpiresAt = expiresAt,
        RevokedAt = null
    };
}