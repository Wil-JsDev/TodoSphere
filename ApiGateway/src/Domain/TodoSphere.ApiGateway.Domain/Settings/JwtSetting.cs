namespace TodoSphere.ApiGateway.Domain.Settings;

public sealed class JwtSetting
{
    public required string Key { get; set; }
    
    public required string Issuer { get; set; }
    
    public required string Audience { get; set; }
}