using TodoSphere.Users.Application.DTOs.Roles;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Mappers;

public static class RolMapper
{
    public static RolesDtOs ToDto(Role role)
    {
        return new RolesDtOs(role.RoleId, role.Name, role.CreatedAt);
    }
}