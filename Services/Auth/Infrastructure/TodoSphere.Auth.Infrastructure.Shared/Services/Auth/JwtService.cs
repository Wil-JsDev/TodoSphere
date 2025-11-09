using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoSphere.Auth.Application.DTOs.Auth;
using TodoSphere.Auth.Application.Factories;
using TodoSphere.Auth.Application.Helpers;
using TodoSphere.Auth.Application.Interfaces.Services;
using TodoSphere.Auth.Application.Interfaces.UnitOfWork;
using TodoSphere.Auth.Application.Utils;
using TodoSphere.Auth.Domain.Common;
using TodoSphere.Auth.Domain.Models;
using TodoSphere.Auth.Domain.Settings;

namespace TodoSphere.Auth.Infrastructure.Shared.Services.Auth;

public class JwtService(
    IOptions<JwtSetting> jwtSetting,
    IUnitOfWork unitOfWork
) : IJwtService
{
    private readonly JwtSetting _jwtSetting = jwtSetting.Value;

    public string GenerateToken(User user)
    {
        ValidateSettings();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("type", TokenType.AccessToken)
        };

        var expiration = TimeSpan.FromMinutes(_jwtSetting.ExpiresInMinutes);

        return BuildToken(claims, expiration);
    }

    public async Task<RefreshToken> GenerateRefreshToken(User user, CancellationToken cancellationToken = default)
    {
        var tokenString = CryptoHelper.GenerateSecureRandomString();

        var expiration = DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpiresInDays);

        var refreshToken = RefreshTokenFactory.Create(tokenString, user.UserId, expiration);

        await unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return refreshToken;
    }

    public async Task<ResultT<AuthenticationResponse>> RefreshTokenAsync(string refreshToken)
    {
        return await RefreshTokensInternal(refreshToken);
    }


    #region Private Methods

    private ResultT<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return ResultT<ClaimsPrincipal>.Failure(Error.BadRequest("400", "token is required"));
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSetting.Key ?? string.Empty)),
            ValidateLifetime = false // Allow expired tokens
        };

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return ResultT<ClaimsPrincipal>.Failure(Error.BadRequest("400", "Invalid token algorithm"));
        }

        return ResultT<ClaimsPrincipal>.Success(principal);
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_jwtSetting.Key) ||
            string.IsNullOrWhiteSpace(_jwtSetting.Issuer) ||
            string.IsNullOrWhiteSpace(_jwtSetting.Audience) ||
            _jwtSetting.ExpiresInMinutes <= 0)
        {
            throw new InvalidOperationException("JWT settings are not properly configured.");
        }
    }

    private async Task<ResultT<AuthenticationResponse>> RefreshTokensInternal(string refreshToken)
    {
        var savedToken = await unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);

        var validateError = ValidateTokenState(savedToken);
        if (validateError is not null)
        {
            return ResultT<AuthenticationResponse>.Failure(validateError);
        }

        var user = await unitOfWork.Users.GetByIdAsync(savedToken.UserId);
        if (user is null)
        {
            return ResultT<AuthenticationResponse>.Failure(Error.NotFound("404",
                "User not found"));
        }

        //Revoke old refresh token
        savedToken.RevokedAt = DateTime.UtcNow;
        unitOfWork.RefreshTokens.Update(savedToken);

        var newToken = GenerateToken(user);

        //Generate a new refresh token and save Db
        var newRefreshToken = GenerateAndAddNewRefreshToken(user);

        await unitOfWork.CompleteAsync();

        AuthenticationResponse response = new(
            AccessToken: newToken,
            RefreshToken: newRefreshToken.Token
        );

        return ResultT<AuthenticationResponse>.Success(response);
    }

    private string BuildToken(IEnumerable<Claim> claims, TimeSpan expiresIn)
    {
        ValidateSettings();
        var key = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSetting.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), // All Claims
            Expires = DateTime.UtcNow.Add(expiresIn),
            Issuer = _jwtSetting.Issuer,
            Audience = _jwtSetting.Audience,
            SigningCredentials = credentials
        };

        // Create and write the token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private RefreshToken GenerateAndAddNewRefreshToken(User user)
    {
        var tokenString = CryptoHelper.GenerateSecureRandomString();
        var expiration = DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpiresInDays);
        var refreshToken = RefreshTokenFactory.Create(tokenString, user.UserId, expiration);

        unitOfWork.RefreshTokens.Add(refreshToken);

        return refreshToken;
    }

    private Error? ValidateTokenState(RefreshToken? savedToken)
    {
        if (savedToken is null)
        {
            return Error.Unauthorized("401", "Refresh token not exist");
        }

        if (savedToken.RevokedAt is not null)
        {
            return Error.Unauthorized("401", "Refresh token is revoked");
        }

        return savedToken.ExpiresAt < DateTime.UtcNow ? Error.Unauthorized("401", "Refresh token is expired") : null;
    }

    #endregion
}