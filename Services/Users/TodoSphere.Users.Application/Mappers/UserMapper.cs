using TodoSphere.Users.Application.DTOs.Users;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Mappers;

public static class UserMapper
{
    public static UserDtOs ToDto(User user)
    {
        return new UserDtOs(
            user.UserId,
            $"{user.FirstName} {user.LastName}",
            user.Email,
            user.PhoneNumber,
            user.Address,
            user.City,
            user.Country,
            user.CreatedAt);
    }

    public static UserGeneralDtOs ToGeneralDto(User user)
    {
        return new UserGeneralDtOs(
            user.UserId,
            $"{user.FirstName} {user.LastName}",
            user.Email,
            user.PhoneNumber,
            user.Address,
            user.City,
            user.Country,
            user.CreatedAt
        );
    }
}