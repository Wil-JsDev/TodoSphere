using System.Security.Cryptography;

namespace TodoSphere.Auth.Application.Helpers;

public static class CryptoHelper
{
    public static string GenerateSecureRandomString(int byteSize = 32)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(byteSize);
        // Convert to a Base64 string
        return Convert.ToBase64String(randomBytes);
    }
}