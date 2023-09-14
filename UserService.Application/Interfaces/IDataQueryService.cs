using System.Threading.Tasks;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IDataQueryService
{
    Task<User?> FindUser(UserId userId);
}