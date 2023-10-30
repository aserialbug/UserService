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
    public Task<Person> this[PersonId id] => Context.Persons.GetAsync(id);
    
    public PersonRepository(EntitiesContext entitiesContext) : base(entitiesContext)
    {
    }

    public async Task Add(Person entity)
    {
        await Context.Persons.AddAsync(entity);
    }

    public async Task Remove(Person entity)
    {
        await Context.Persons.DeleteAsync(entity);
    }
}