namespace TodoSphere.Auth.Domain.Models;

public sealed class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    
    public required string Token { get; set; }
    public Guid UserId { get; set; }

    public DateTime ExpiresAt { get; set; }
    
    public DateTime? RevokedAt { get; set; }

    public User User { get; set; }
}