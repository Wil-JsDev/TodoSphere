using TodoSphere.Auth.Application.DTOs.Auth;
using TodoSphere.Auth.Application.Interfaces.Services;
using TodoSphere.Auth.Application.Interfaces.UnitOfWork;
using TodoSphere.Auth.Application.Utils;

namespace TodoSphere.Auth.Application.Services;

public sealed class AuthenticationService(
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork
) : IAuthenticationService
{
    public async Task<ResultT<AuthenticationResponse>> AuthenticationAsync(string email, string password)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(email);
        if (user is null)
            return ResultT<AuthenticationResponse>.Failure(Error.NotFound("404", "User not found."));

        if (!passwordHasher.Verify(password, user.PasswordHash))
        {
            return ResultT<AuthenticationResponse>.Failure(Error.Unauthorized("401", "Invalid password."));
        }

        var accessToken = jwtService.GenerateToken(user);
        var refreshToken = await jwtService.GenerateRefreshToken(user);

        var response = new AuthenticationResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token
        );

        return ResultT<AuthenticationResponse>.Success(response);
    }

    public async Task<ResultT<AuthenticationResponse>> RefreshTokenAsync(string refreshToken)
    {
        return await jwtService.RefreshTokenAsync(refreshToken);
    }
}