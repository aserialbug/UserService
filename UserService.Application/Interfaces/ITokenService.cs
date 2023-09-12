using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface ITokenService
{
    Task<Token> GetToken(User user);
}