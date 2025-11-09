using TodoSphere.Auth.Application.DTOs.Auth;
using TodoSphere.Auth.Application.Utils;

namespace TodoSphere.Auth.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<ResultT<AuthenticationResponse>> AuthenticationAsync(string email, string password);
    
    Task<ResultT<AuthenticationResponse>> RefreshTokenAsync(string refreshToken);
}