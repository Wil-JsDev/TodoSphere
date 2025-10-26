using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Factories;

public static class RolFactory
{
    public static Roles Create(string name)
    {
        return new Roles
        {
            RoleId = Guid.NewGuid(),
            Name = name
        };
    }
    
    public static Roles Update(Roles role, string name)
    {
        role.Name = name;
        
        return role;
    }
}