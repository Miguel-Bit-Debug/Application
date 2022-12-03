using Application.Domain.Models;
using Application.Domain.Repositories;
using Application.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Infra.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountRepository(UserManager<Account> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public IEnumerable<object> GetAllAccounts()
    {
        var users = _userManager.Users.Select(x => new { Name = x.Avatar });

        return users;
    }

    public async Task<string> LoginAccount(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, true);

        if(!result.Succeeded)
        {
            return string.Empty;
        }

        var token = _tokenService.GenerateAcessToken(email);

        return token;
    }

    public async Task<string> SaveAccount(Account account, string password)
    {
        var result = await _userManager.CreateAsync(account, password);

        if(!result.Succeeded)
            return string.Empty;

        var token = _tokenService.GenerateAcessToken(account.Email);
        return token;
    }
}
