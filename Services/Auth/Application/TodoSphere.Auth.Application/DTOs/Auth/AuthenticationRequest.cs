namespace TodoSphere.Auth.Application.DTOs.Auth;

public sealed record AuthenticationRequest(string Email, string Password);