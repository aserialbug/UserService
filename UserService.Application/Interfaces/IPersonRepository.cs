using UserService.Domain.Person;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IPersonRepository : IRepository<Person, UserId>
{
    
}