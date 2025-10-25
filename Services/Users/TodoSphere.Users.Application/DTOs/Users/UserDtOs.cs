namespace TodoSphere.Users.Application.DTOs.Users;

public sealed record UserDtOs(
    Guid UserId,
    string FullName,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string Country,
    DateTime CreatedAt
);