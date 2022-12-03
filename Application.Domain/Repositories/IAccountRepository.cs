using Application.Domain.Models;

namespace Application.Domain.Repositories;

public interface IAccountRepository
{
    Task<string> SaveAccount(Account account, string password);
    Task<string> LoginAccount(string email, string password);
    IEnumerable<object> GetAllAccounts();
}
