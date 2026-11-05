using EcoStepWeb.API.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoStepWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config, IJwtKeyService jwtKeyService) : ControllerBase
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

        var tokenString = jwtKeyService.GetTokenString(request.UserName, request.Password);

        return Ok(new { Token = tokenString });
    }
}
