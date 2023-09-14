using System.Threading.Tasks;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IHashingService
{
    Task<Password> Encrypt(ClearTextPassword password, Salt? salt = null);
}