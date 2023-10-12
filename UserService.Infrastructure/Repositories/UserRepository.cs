using System.Threading.Tasks;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.User;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Repositories;

internal class UserRepository : BaseRepository, IUserRepository
{
    private readonly PepperService _pepperService;

    public UserRepository(PepperService pepperService, PostgreSqlContext context) : base(context)
    {
        _pepperService = pepperService;
    }

    public Task<User> this[UserId id] => GetById(id);

    private async Task<User> GetById(UserId id)
    {
        return await DataSource.FindUserById(id, _pepperService)
            ?? throw new NotFoundException($"User with id={id} was not found");
    }

    public async Task Add(User entity)
    {
        await DataSource.AddUser(entity, _pepperService);
    }

    public async Task Remove(User entity)
    {
        await DataSource.RemoveUser(entity.Id);
    }
}