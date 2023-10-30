using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Friends;

public abstract class FriendshipDomainEvent : DomainEvent
{
    public UserId From { get; }
    public UserId To { get; }
    
    public FriendshipDomainEvent(DomainEventId id, DateTime createdAt, UserId from, UserId to) : base(id, createdAt)
    {
        From = from;
        To = to;
    }
}