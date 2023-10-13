using System.Threading.Tasks;
using Npgsql;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Services;

internal class DataQueryService : IDataQueryService
{
    private readonly PepperService _pepperService;
    
    private NpgsqlDataSource DataSource { get; }

    public DataQueryService(PostgreSqlContext context, PepperService pepperService)
    {
        DataSource = context.Standby;
        _pepperService = pepperService;
    }

    public async Task<User?> FindUser(UserId userId)
    {
        return await DataSource.FindUserById(userId, _pepperService);
    }

    public async Task<PersonViewModel?> FindPerson(UserId userId)
    {
        return await DataSource.FindPersonById(userId);
    }

    public async Task<PersonViewModel[]> SearchPersons(string firstName, string lastName)
    {
        return await DataSource.SearchByName(firstName, lastName);
    }
}