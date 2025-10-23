namespace TodoSphere.Auth.Domain.Models;

public sealed class User
{
    public Guid UserId { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string Roles { get; set; }

    public bool IsEmailVerified { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}