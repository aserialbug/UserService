using UserService.Application.Interfaces;
using UserService.Domain.User;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Repositories;

internal class UserRepository : BaseRepository, IUserRepository
{
    private readonly PepperService _pepperService;
    private readonly EntitiesContext _entitiesContext;

    public UserRepository(PepperService pepperService, PostgresContext postgreSqlContext, EntitiesContext entitiesContext) : base(postgreSqlContext)
    {
        _pepperService = pepperService;
        _entitiesContext = entitiesContext;
    }

    public Task<User> this[UserId id] => _entitiesContext.Users.GetAsync(id);

    public async Task Add(User entity)
    {
        await _entitiesContext.Users.AddAsync(entity);
    }

    public async Task Remove(User entity)
    {
        await _entitiesContext.Users.DeleteAsync(entity);
    }
}