using UserService.Application.Interfaces;
using UserService.Domain.Friends;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class FriendshipRepository : BaseRepository, IFriendshipRepository
{
    public Task<Friendship> this[FriendshipId id] 
        => DataSource.GetFriendship(id);
    
    public FriendshipRepository(PostgreSqlContext postgreSqlContext) : base(postgreSqlContext)
    {
    }

    public async Task Add(Friendship entity)
    {
        await DataSource.AddFriendship(entity);
    }

    public async Task Remove(Friendship entity)
    {
        await DataSource.RemoveFriendship(entity.Id);
    }
}