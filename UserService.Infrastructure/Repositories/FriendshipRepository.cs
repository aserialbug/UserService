using UserService.Application.Interfaces;
using UserService.Domain.Friends;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class FriendshipRepository : BaseRepository, IFriendshipRepository
{
    public Task<Friendship> this[FriendshipId id] => Context.Friendships.GetAsync(id);
    
    public FriendshipRepository(EntitiesContext entitiesContext) : base(entitiesContext)
    {
    }

    public async Task Add(Friendship entity)
    {
        await Context.Friendships.AddAsync(entity);
    }

    public async Task Remove(Friendship entity)
    {
        await Context.Friendships.DeleteAsync(entity);
    }
}