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
}