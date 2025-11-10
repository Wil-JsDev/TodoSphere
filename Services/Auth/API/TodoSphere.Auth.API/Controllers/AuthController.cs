using Microsoft.AspNetCore.Mvc;
using TodoSphere.Auth.API.Common;
using TodoSphere.Auth.Application.DTOs.Auth;
using TodoSphere.Auth.Application.Interfaces.Services;
using TodoSphere.Auth.Application.Utils;

namespace TodoSphere.Auth.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<ResultT<AuthenticationResponse>>> LoginAsync(
        [FromBody] AuthenticationRequest authenticationRequest)
    {
        var result = await authenticationService.AuthenticationAsync(
            email: authenticationRequest.Email,
            password: authenticationRequest.Password
        );

        return result.IsSuccess ? Ok(result.Value) : HandlerError.Handle(result.Error);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ResultT<AuthenticationResponse>>> RefreshTokenAsync(
        [FromBody] RefreshRequest refreshRequest)
    {
        var result = await authenticationService.RefreshTokenAsync(refreshRequest.RefreshToken);
        return result.IsSuccess ? Ok(result.Value) : HandlerError.Handle(result.Error);
    }
}