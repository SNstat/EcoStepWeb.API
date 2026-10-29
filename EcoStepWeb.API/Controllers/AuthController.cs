using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcoStepWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    public record LoginRequest(string UserName, string Password);

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
            return BadRequest("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password is required.");


        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, request.UserName),
                new Claim(ClaimTypes.Name, request.UserName) // Fallback for standard mapping
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}
