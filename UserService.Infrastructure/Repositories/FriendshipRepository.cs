using UserService.Application.Interfaces;
using UserService.Domain.Friends;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class FriendshipRepository : BaseRepository, IFriendshipRepository
{
    public FriendshipRepository(PostgresContext postgreSqlContext) : base(postgreSqlContext)
    {
    }

    public Task<Friendship> this[FriendshipId id]
        => GetById(id);

    private async Task<Friendship> GetById(FriendshipId id)
    {
        var friendship  = await DataSource.GetFriendship(id);
        return friendship;
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