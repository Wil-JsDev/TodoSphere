using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Factories;

public static class UserFactory
{
    public static User Create(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string address,
        string city,
        string country
    )
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            City = city,
            Country = country
        };
    }
}