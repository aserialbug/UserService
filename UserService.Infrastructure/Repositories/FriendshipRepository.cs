using UserService.Application.Interfaces;
using UserService.Domain.Common;
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
        await Context.DomainEvents.Register(new FriendshipCreatedDomainEvent(DomainEventId.New(), DateTime.Now,
            entity.Id.From, entity.Id.To));
    }

    public async Task Remove(Friendship entity)
    {
        await Context.Friendships.DeleteAsync(entity);
        await Context.DomainEvents.Register(new FriendshipDeletedDomainEvent(DomainEventId.New(), DateTime.Now,
            entity.Id.From, entity.Id.To));
    }
}