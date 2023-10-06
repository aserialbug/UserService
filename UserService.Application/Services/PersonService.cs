using UserService.Application.Interfaces;
using UserService.Domain.Person;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class PersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Person> GetById(UserId id)
    {
        var person = await _personRepository[id];
        return person;
    }
}