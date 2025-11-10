namespace TodoSphere.Auth.Application.DTOs.Auth;

public sealed record AuthenticationResponse(
    string AccessToken,
    string RefreshToken
);