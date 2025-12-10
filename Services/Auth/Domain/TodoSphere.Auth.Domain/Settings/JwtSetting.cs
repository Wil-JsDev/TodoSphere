namespace TodoSphere.Auth.Domain.Settings;

public sealed class JwtSetting
{
    public required string Key { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required int ExpiresInMinutes { get; set; }

    public required int RefreshTokenExpiresInDays { get; set; }
}