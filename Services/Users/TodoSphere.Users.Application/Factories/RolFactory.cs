using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Factories;

public static class RolFactory
{
    public static Role Create(string name)
    {
        return new Role
        {
            RoleId = Guid.NewGuid(),
            Name = name
        };
    }

    public static Role Update(Role role, string name)
    {
        role.Name = name;
        role.UpdatedAt = DateTime.UtcNow;
        return role;
    }
}