using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Person;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class PersonRepository : BaseRepository, IPersonRepository
{
    public PersonRepository(PostgreSqlContext context) : base(context)
    {
    }

    public Task<Person> this[UserId id] => GetById(id);

    public async Task<Person> GetById(UserId userId)
    {
        return await DataSource.FindPersonById(userId)
            ?? throw new NotFoundException($"Person with id={userId} was not found");
    }

    public async Task Add(Person entity)
    {
        await DataSource.AddPerson(entity);
    }

    public async Task Remove(Person entity)
    {
        await DataSource.RemovePerson(entity.Id);
    }
}