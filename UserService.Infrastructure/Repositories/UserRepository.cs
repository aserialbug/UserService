using UserService.Application.Interfaces;
using UserService.Domain.User;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Repositories;

internal class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(EntitiesContext entitiesContext) : base(entitiesContext)
    {
    }

    public Task<User> this[UserId id] => Context.Users.GetAsync(id);

    public async Task Add(User entity)
    {
        await Context.Users.AddAsync(entity);
    }

    public async Task Remove(User entity)
    {
        await Context.Users.DeleteAsync(entity);
    }
}