using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Person;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class PersonRepository : BaseRepository, IPersonRepository
{
    public PersonRepository(PostgresContext postgreSqlContext) : base(postgreSqlContext)
    {
    }

    public Task<Person> this[PersonId id] => GetById(id);
    
    private async Task<Person> GetById(PersonId id)
    {
        var person  = await DataSource.GetPersonById(id);
        return person;
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