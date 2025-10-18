using TodoSphere.Users.Domain.Base;

namespace TodoSphere.Users.Domain.Models;

public sealed class User : CreationAndUpdateDate
{
    public Guid UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Address { get; set; }

    public required string City { get; set; }

    public required string Country { get; set; }

    // JSON 
    public ICollection<UserRoleInfo> Roles { get; set; } = new List<UserRoleInfo>();
}