namespace TodoSphere.Users.Domain.Models;

public sealed class UserRoleInfo
{
    public Guid RoleId { get; set; }

    public required string RoleName { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}