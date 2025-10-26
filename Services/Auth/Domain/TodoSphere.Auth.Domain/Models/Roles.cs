namespace TodoSphere.Auth.Domain.Models;

public sealed class Roles
{
    public Guid RoleId { get; set; }
    public required string Name { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<User> Users { get; set; } = new List<User>();
}