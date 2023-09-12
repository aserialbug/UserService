using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IUserRepository : IRepository<User, UserId>
{
}