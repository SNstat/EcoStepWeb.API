using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EcoStepWeb.API.Authentication;

public interface IJwtKeyService
{
    public string GetJwtKey();
    public string? GetTokenString(string username, string password);
}

public sealed class JwtKeyService : IJwtKeyService
{
    private readonly string _jwtKey;
    private readonly IConfiguration _configuration;

    public JwtKeyService(IConfiguration configuration)
    {
        _jwtKey = GenerateSecureKey();
        _configuration = configuration;
    }

    public string GetJwtKey()
    {
        return _jwtKey;
    }

    public string? GetTokenString(string username, string password)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, password),
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    private string GenerateSecureKey()
    {
        var randomBytes = new byte[48];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }
}
