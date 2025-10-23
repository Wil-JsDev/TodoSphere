using TodoSphere.Users.Domain.Base;

namespace TodoSphere.Users.Domain.Models;

public sealed class Role : CreationAndUpdateDate
{
    public Guid RoleId { get; set; }

    public required string Name { get; set; }
}