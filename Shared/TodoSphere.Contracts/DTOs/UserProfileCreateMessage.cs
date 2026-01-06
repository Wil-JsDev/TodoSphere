namespace TodoSphere.Contracts.DTOs;

/// <summary>
/// Represents a message for creating a user profile within the system.
/// </summary>
public sealed record UserProfileCreateMessage(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string Country
);