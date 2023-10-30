using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Friends;

public class FriendshipCreatedDomainEvent : FriendshipDomainEvent
{
    public FriendshipCreatedDomainEvent(DomainEventId id, DateTime createdAt, UserId from, UserId to) : base(id, createdAt, from, to)
    {
    }
}