using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Person;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class PersonRepository : IPersonRepository
{
    private readonly PostgreSqlContext _context;

    public PersonRepository(PostgreSqlContext context)
    {
        _context = context;
    }

    public Task<Person> this[UserId id] => GetById(id);

    public async Task<Person> GetById(UserId userId)
    {
        return await _context.DataSource.FindPersonById(userId)
            ?? throw new NotFoundException($"Person with id={userId} was not found");
    }

    public async Task Add(Person entity)
    {
        await _context.DataSource.AddPerson(entity);
    }

    public async Task Remove(Person entity)
    {
        await _context.DataSource.RemovePerson(entity.Id);
    }
}