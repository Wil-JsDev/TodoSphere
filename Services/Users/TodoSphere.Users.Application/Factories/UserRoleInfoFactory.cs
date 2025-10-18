using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Factories;

public static class UserRoleInfoFactory
{
    public static UserRoleInfo Create(Guid roleId, string roleName)
    {
        return new UserRoleInfo
        {
            RoleId = roleId,
            RoleName = roleName
        };
    }
}