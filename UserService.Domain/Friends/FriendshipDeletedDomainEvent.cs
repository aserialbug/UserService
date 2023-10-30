using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Friends;

public class FriendshipDeletedDomainEvent : FriendshipDomainEvent
{
    public FriendshipDeletedDomainEvent(DomainEventId id, DateTime createdAt, UserId from, UserId to) : base(id, createdAt, from, to)
    {
    }
}