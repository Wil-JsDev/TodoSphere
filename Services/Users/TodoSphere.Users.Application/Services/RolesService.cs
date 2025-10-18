using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.DTOs.Roles;
using TodoSphere.Users.Application.Factories;
using TodoSphere.Users.Application.Helpers;
using TodoSphere.Users.Application.Interfaces.Repositories;
using TodoSphere.Users.Application.Interfaces.Services;
using TodoSphere.Users.Application.Mappers;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Application.Utils;

namespace TodoSphere.Users.Application.Services;

public class RolesService(IUnitOfWork unitOfWork) : IRolesService
{
    public async Task<ResultT<RolesDtOs>> CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        if (await unitOfWork.Roles.ExistsByNameAsync(name, cancellationToken))
            return ResultT<RolesDtOs>.Failure(Error.Conflict("409", "Role already exists"));

        if (string.IsNullOrEmpty(name))
            return ResultT<RolesDtOs>.Failure(Error.Failure("400", "Role name cannot be empty"));

        var role = RolFactory.Create(name);

        await unitOfWork.Roles.AddAsync(role, cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return ResultT<RolesDtOs>.Success(RolMapper.ToDto(role));
    }

    public async Task<ResultT<RolesDtOs>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var role = await unitOfWork.Roles.GetByNameAsync(name, cancellationToken);

        return role is null
            ? ResultT<RolesDtOs>.Failure(Error.NotFound("404", "Role not found"))
            : ResultT<RolesDtOs>.Success(RolMapper.ToDto(role));
    }

    public async Task<ResultT<RolesDtOs>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id, cancellationToken);

        return role is null
            ? ResultT<RolesDtOs>.Failure(Error.NotFound("404", "Role not found"))
            : ResultT<RolesDtOs>.Success(RolMapper.ToDto(role));
    }

    public async Task<ResultT<PagedResult<RolesDtOs>>> GetPagedRolesAsync(PaginationParameter paginationParameter,
        CancellationToken cancellationToken = default)
    {
        var paginationValidation = PaginationHelper<RolesDtOs>.PaginationError(paginationParameter);

        if (!paginationValidation.IsSuccess) return ResultT<PagedResult<RolesDtOs>>.Failure(paginationValidation.Error);

        var pagedResult = await unitOfWork.Roles.GetPagedRolesAsync(paginationParameter, cancellationToken);

        if (!pagedResult.Items.Any())
            return ResultT<PagedResult<RolesDtOs>>.Failure(Error.NotFound("404", "Roles not found"));

        var dtoItems = pagedResult.Items.Select(RolMapper.ToDto).ToList();

        PagedResult<RolesDtOs> pagedResultDto = new()
        {
            Items = dtoItems,
            TotalItems = pagedResult.TotalItems,
            CurrentPage = pagedResult.CurrentPage,
            TotalPages = pagedResult.TotalPages,
        };

        return ResultT<PagedResult<RolesDtOs>>.Success(pagedResultDto);
    }

    public async Task<ResultT<RolesDtOs>> UpdateAsync(Guid id, string name,
        CancellationToken cancellationToken = default)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id, cancellationToken);
        if (role is null)
            return ResultT<RolesDtOs>.Failure(Error.NotFound("404", "Role not found"));

        var roleUpdate = RolFactory.Update(role, name);

        unitOfWork.Roles.Update(roleUpdate);

        await unitOfWork.CompleteAsync(cancellationToken);

        return ResultT<RolesDtOs>.Success(RolMapper.ToDto(roleUpdate));
    }

    public async Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id, cancellationToken);
        if (role is null)
            return Result.Failure(Error.NotFound("404", "Role not found"));

        unitOfWork.Roles.Remove(role);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}