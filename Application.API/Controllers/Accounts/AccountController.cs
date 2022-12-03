using Application.Domain.DTOs.Request;
using Application.Domain.Models;
using Application.Domain.Repositories;
using Application.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers.Accounts;

[Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _accountRepository.GetAllAccounts();

        return Ok(users);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new Account
        {
            Avatar = request.Name,
            UserName = request.Email,
            Email = request.Email,
            IsAdmin = request.IsAdmin,
            EmailConfirmed = true
        };
        
        var result = await _accountRepository.SaveAccount(user, request.Password);
        
        if(string.IsNullOrEmpty(result))
        {
            return BadRequest("Failed save account");
        }

        return Ok(new { token = result });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _accountRepository.LoginAccount(request.Email, request.Password);

        if (string.IsNullOrEmpty(result))
        {
            return BadRequest("Email or Password is incorrect");
        }

        return Ok(new { token = result });
    }
}
