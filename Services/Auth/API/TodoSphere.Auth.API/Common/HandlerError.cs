using Microsoft.AspNetCore.Mvc;
using TodoSphere.Auth.Application.Utils;

namespace TodoSphere.Auth.API.Common;

public static class HandlerError
{
    public static ActionResult Handle(Error errorResult)
    {
        return errorResult.Code switch
        {
            "401" => new UnauthorizedObjectResult(errorResult),
            "404" => new NotFoundObjectResult(errorResult),
            _ => new BadRequestObjectResult(errorResult)
        };
    }
}