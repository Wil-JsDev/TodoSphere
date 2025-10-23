using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.DTOs.Users;
using TodoSphere.Users.Application.Factories;
using TodoSphere.Users.Application.Helpers;
using TodoSphere.Users.Application.Interfaces.Repositories;
using TodoSphere.Users.Application.Interfaces.Services;
using TodoSphere.Users.Application.Mappers;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Application.Utils;
using TodoSphere.Users.Domain.Enum;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Application.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<ResultT<UserDtOs>> CreateAsync(CreateUserDtOs createUser,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(createUser.Email))
            return ResultT<UserDtOs>.Failure(Error.Failure("400", "Email cannot be empty"));

        if (await unitOfWork.Users.ExistsByEmailAsync(createUser.Email, cancellationToken))
            return ResultT<UserDtOs>.Failure(Error.Conflict("409", "User with this email already exists"));

        var user = UserFactory.Create(
            createUser.FirstName,
            createUser.LastName,
            createUser.Email,
            createUser.PhoneNumber,
            createUser.Address,
            createUser.City,
            createUser.Country
        );

        var defaultRole = await unitOfWork.Roles.GetByNameAsync(Roles.User.ToString(), cancellationToken);

        if (defaultRole is null)
            return ResultT<UserDtOs>.Failure(Error.Failure("503",
                "Service configuration error: Default role not found."));

        user.Roles.Add(UserRoleInfoFactory.Create(defaultRole.RoleId, defaultRole.Name));

        await unitOfWork.Users.AddAsync(user, cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        var userDto = UserMapper.ToDto(user);

        return ResultT<UserDtOs>.Success(userDto);
    }

    public async Task<ResultT<PagedResult<UserDtOs>>> GetPagedUsersAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken = default)
    {
        var paginationValidation = PaginationHelper<User>.PaginationError(paginationParameter);

        if (!paginationValidation.IsSuccess) return ResultT<PagedResult<UserDtOs>>.Failure(paginationValidation.Error);

        var pagedResult = await unitOfWork.Users.GetPagedUsersAsync(paginationParameter, cancellationToken);

        if (!pagedResult.Items.Any())
            return ResultT<PagedResult<UserDtOs>>.Failure(Error.NotFound("404", "Users not found"));

        var items = pagedResult.Items.Select(UserMapper.ToDto).ToList();

        PagedResult<UserDtOs> pagedResponseDto = new()
        {
            Items = items,
            TotalItems = pagedResult.TotalItems,
            CurrentPage = pagedResult.CurrentPage,
            TotalPages = pagedResult.TotalPages,
        };

        return ResultT<PagedResult<UserDtOs>>.Success(pagedResponseDto);
    }

    public async Task<Result> AssignRoleToUserAsync(Guid userId, Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure(Error.NotFound("404", "User not found"));

        var role = await unitOfWork.Roles.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            return Result.Failure(Error.NotFound("404", "Role not found"));

        if (user.Roles.Any(r => r.RoleId == roleId))
            return Result.Failure(Error.Conflict("409", "User already has this role assigned"));

        var newRoleInfo = UserRoleInfoFactory.Create(roleId, role.Name);

        user.Roles.Add(newRoleInfo);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RemoveRoleFromUserAsync(Guid userId, Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure(Error.NotFound("404", "User not found"));

        var role = await unitOfWork.Roles.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            return Result.Failure(Error.NotFound("404", "Role not found"));

        var roleInfo = user.Roles.FirstOrDefault(r => r.RoleId == roleId);

        if (roleInfo is null)
            return Result.Failure(Error.NotFound("404", "User does not have this role assigned"));

        user.Roles.Remove(roleInfo);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}