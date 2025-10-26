namespace TodoSphere.Auth.Domain.Models;

public sealed class User
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public bool IsEmailVerified { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public Roles Role { get; set; }
}