namespace TodoSphere.Users.Application.DTOs.Users;

public sealed record CreateUserDtOs(
    string FirstName,
    string LastName,
    string Password,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string Country
);