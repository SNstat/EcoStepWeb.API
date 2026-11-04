using System.Security.Cryptography;

namespace EcoStepWeb.API.Jwt;

internal static class JwtKeyGenerator
{
    internal static string GenerateSecureKey()
    {
        var randomBytes = new byte[48];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }
}