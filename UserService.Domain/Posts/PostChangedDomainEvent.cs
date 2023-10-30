using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Posts;

public class PostChangedDomainEvent : PostDomainEvent
{
    public PostChangedDomainEvent(DomainEventId id, DateTime createdAt, PostId postId, UserId authorId) : base(id, createdAt, postId, authorId)
    {
    }
}