using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Application.Factories;

public static class UserFactory
{
    public static User Create(string email, string password, Guid roleId)
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            Email = email,
            PasswordHash = password,
            RoleId = roleId,
            IsEmailVerified = true
        };
    }
}