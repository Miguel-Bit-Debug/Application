using Application.Domain.DTOs;
using Application.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.API.Controllers.Security;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<Account> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationController(IConfiguration configuration,
                                    UserManager<Account> manager)
    {
        _configuration = configuration;
        _userManager = manager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new Account
        {
            Avatar = request.Name,
            UserName = request.Email,
            Email = request.Email,
            IsAdmin = request.IsAdmin
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var token = GenerateAcessToken(request.Email);
        return Ok(new { token = token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var token = GenerateAcessToken(request.Email);
        return Ok(new { token = token });
    }

    private string GenerateAcessToken(string email)
    {
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity (
                new Claim[] 
                {
                    new Claim(ClaimTypes.Email, email)
                }),

            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(_configuration["SecretKey"])),
                        SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration["Audience"],
            Issuer = _configuration["Issuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}
