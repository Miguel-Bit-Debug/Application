namespace Application.Domain.Services;

public interface ITokenService
{
    string GenerateAcessToken(string email);
}
