using TodoSphere.Users.Application.DTOs.Common;
using TodoSphere.Users.Application.Pagination;
using TodoSphere.Users.Application.Utils;

namespace TodoSphere.Users.Application.Helpers;

public static class PaginationHelper<TDto>
{
    public static ResultT<PagedResult<TDto>> PaginationError(PaginationParameter parameter)
    {
        if (parameter.PageNumber <= 0 || parameter.PageSize <= 0)
            return ResultT<PagedResult<TDto>>.Failure(Error.Failure("400",
                "Page number and page size must be greater than 0"));

        return ResultT<PagedResult<TDto>>.Success(new PagedResult<TDto>());
    }
}