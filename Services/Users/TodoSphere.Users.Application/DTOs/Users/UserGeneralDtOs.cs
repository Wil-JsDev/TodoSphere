namespace TodoSphere.Users.Application.DTOs.Users;

public sealed record UserGeneralDtOs(
    Guid UserId,
    string FullName,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string Country,
    DateTime CreatedAt
);